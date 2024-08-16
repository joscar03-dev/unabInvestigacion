using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public class CloudStorageService : ICloudStorageService
    {
        private readonly CloudBlobClient _blobClient;
        private readonly CloudStorageCredentials _settings;

        public CloudStorageService(IOptions<CloudStorageCredentials> settings)
        {
            _settings = settings.Value;

            var storageAccount = new CloudStorageAccount(
                new StorageCredentials(
                    _settings.StorageName,
                    _settings.AccessKey), true);

            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        private async Task<string> Upload(Stream stream, string cloudStorageContainer, string fileName, string systemFolder = null, bool isUserImage = false)
        {
            if (cloudStorageContainer != "bit4idtest")
                if (GeneralConstants.FileStorage.STORAGE_MODE == FileStorageConstants.Mode.SERVER_STORAGE_MODE || isUserImage)
                {
                    var yearFolder = DateTime.Today.Year.ToString();
                    var projectFolderPath = "";
                    var relativePath = "";

                    if (string.IsNullOrEmpty(systemFolder))
                    {
                        projectFolderPath = Path.Combine(GeneralConstants.FileStorage.PATH, yearFolder, cloudStorageContainer);
                        relativePath = Path.Combine(yearFolder, cloudStorageContainer);
                    }
                    else
                    {
                        projectFolderPath = Path.Combine(GeneralConstants.FileStorage.PATH, yearFolder, systemFolder, cloudStorageContainer);
                        relativePath = Path.Combine(yearFolder, systemFolder, cloudStorageContainer);
                    }

                    var filePath = Path.Combine(projectFolderPath, fileName);
                    relativePath = Path.Combine(relativePath, fileName);

                    Directory.CreateDirectory(projectFolderPath);

                    using (var newstream = new FileStream(filePath, FileMode.Create))
                    {
                        await stream.CopyToAsync(newstream);
                    }

                    return relativePath;
                }

            var container = _blobClient.GetContainerReference(cloudStorageContainer);

            if (await container.CreateIfNotExistsAsync())
            {
                await container.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                );
            }

            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromStreamAsync(stream);

            return blockBlob.Uri.ToString().Replace("%5C", "/");
        }

        public async Task<string> UploadFile(Stream stream, string container, string fileName, string extension, string systemFolder = null, bool isUserImage = false)
        {
            return await Upload(stream, container, $"{fileName}{extension}", systemFolder, isUserImage: isUserImage);
        }

        public async Task<string> UploadFile(Stream stream, string container, string extension, string systemFolder = null)
        {
            return await Upload(stream, container, $"{Guid.NewGuid()}{extension}", systemFolder);
        }
       
        public async Task<string> UploadProductBinary(Stream stream, string container, string systemFolder = null)
        {
            return await Upload(stream, container, Guid.NewGuid().ToString(), systemFolder);
        }

        private async Task<bool> Delete(string fileName, string cloudStorageContainer)
        {
            if (cloudStorageContainer != "bit4idtest")
            {
                if (GeneralConstants.FileStorage.STORAGE_MODE == FileStorageConstants.Mode.SERVER_STORAGE_MODE)
                {
                    var filePath = Path.Combine(GeneralConstants.FileStorage.PATH, fileName);
                    if (File.Exists(filePath)) File.Delete(filePath);
                    return true;
                }
            }

            fileName = fileName.Split('/').Last();
            var container = _blobClient.GetContainerReference(cloudStorageContainer);
            var blockBlob = container.GetBlockBlobReference(fileName);

            return await blockBlob.DeleteIfExistsAsync();
        }

        public async Task<bool> TryDeleteProductBinary(string fileName)
        {
            return await Delete(fileName, "binaries");
        }

        private async Task<Stream> Download(Stream stream, string cloudStorageContainer, string fileName, bool folderForUserImages = false)
        {
            Uri uriResult;

            if (!fileName.StartsWith("https://") && fileName.StartsWith("https:/"))
            {
                fileName = fileName.Substring(7);
                fileName = "https://" + fileName;
            }

            var result = Uri.TryCreate(fileName, UriKind.Absolute, out uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            if ((GeneralConstants.FileStorage.STORAGE_MODE == FileStorageConstants.Mode.SERVER_STORAGE_MODE || folderForUserImages) && !result)
            {
                var filePath = Path.Combine(GeneralConstants.FileStorage.PATH, fileName);
                if (File.Exists(filePath))
                {
                    using (var file = new FileStream(filePath, FileMode.Open))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return stream;
                }
                return null;
            }
            else
            {
                if (result) cloudStorageContainer = fileName.Split('/')[3];

                fileName = fileName.Split($"/{cloudStorageContainer}/").Last();

                var container = _blobClient.GetContainerReference(cloudStorageContainer);
                var blockBlob = container.GetBlockBlobReference(fileName);
                await blockBlob.DownloadToStreamAsync(stream);
                return stream;
            }
        }

        public async Task<Stream> TryDownload(Stream stream, string cloudStorageContainer, string filePath, bool folderForUserImages = false)
        {
            return await Download(stream, cloudStorageContainer, filePath, folderForUserImages);
        }

        public async Task<HashSet<(string, MemoryStream)>> DownloadFilesInBulk(string cloudStorageContainer, List<string> fileReferences, List<string> fileNames)
        {
            var listMStream = new HashSet<(string, MemoryStream)>();
            if (GeneralConstants.FileStorage.STORAGE_MODE == FileStorageConstants.Mode.SERVER_STORAGE_MODE)
            {
                foreach (var item in fileReferences.Select((value, i) => new { i, value }))
                {
                    var filePath = Path.Combine(GeneralConstants.FileStorage.PATH, item.value);
                    if (File.Exists(filePath))
                    {
                        var mStream = new MemoryStream();
                        using (var file = new FileStream(filePath, FileMode.Open))
                        {
                            await file.CopyToAsync(mStream);
                        }
                        listMStream.Add(($"{fileNames[item.i]}", mStream));
                    }
                }
            }
            else
            {
                var container = _blobClient.GetContainerReference(cloudStorageContainer);

                foreach (var item in fileReferences.Select((value, i) => new { i, value }))
                {
                    var fileName = item.value.Split($"/{cloudStorageContainer}/").Last();
                    var blockBlob = container.GetBlockBlobReference(fileName);
                    var mStream = new MemoryStream();
                    await blockBlob.DownloadToStreamAsync(mStream);
                    listMStream.Add(($"{fileNames[item.i]}", mStream));
                }
            }
            return listMStream;
        }

        public async Task<MemoryStream> DownloadImage(string container, string filename)
        {
            var mStream = new MemoryStream();

            var stream = await Download(mStream, container, filename);

            await stream.CopyToAsync(mStream);

            return mStream;
        }

        private async Task<String> UploadBase64(String imageToUpload, String cloudStorageContainer, String fileName)
        {
            try
            {
                MemoryStream streams = new MemoryStream();
                CloudBlobContainer container = _blobClient.GetContainerReference(cloudStorageContainer);
                if (await container.CreateIfNotExistsAsync())
                {
                    await container.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                    );
                }
                string convert = imageToUpload;
                convert = convert.Replace("data:image/jpeg;base64,", String.Empty);
                convert = convert.Replace("data:image/png;base64,", String.Empty);
                convert = convert.Replace("data:image/bmp;base64,", String.Empty);
                string header = imageToUpload.Replace(convert, String.Empty);
                string type = (header.Replace("data:image/", String.Empty)).Replace(";base64,", String.Empty);
                string type2 = (header.Replace("data:", String.Empty)).Replace(";base64,", String.Empty);
                Image imagen = ConvertHelpers.Base64ToImage(convert);
                var partname = DateTime.UtcNow.ToDefaultTimeZone().Ticks.ToString();
                string uniqueBlobName = string.Format("image_{0}." + type, partname);
                CloudBlockBlob blob = container.GetBlockBlobReference(uniqueBlobName);
                blob.Properties.ContentType = type2;
                Image imgs = imagen;
                if (type == "jpeg")
                    imgs.Save(streams, ImageFormat.Jpeg);
                if (type == "png")
                    imgs.Save(streams, ImageFormat.Png);
                if (type == "bmp")
                    imgs.Save(streams, ImageFormat.Bmp);
                byte[] imageBytes = streams.GetBuffer();
                await blob.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);
                imgs.Dispose();
                streams.Dispose();
                return blob.StorageUri.PrimaryUri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
        }

        public async Task<String> UploadFileBase64(String imageToUpload, String container, String filename)
        {
            return await UploadBase64(imageToUpload, container, filename);
        }

        public async Task<bool> TryDelete(string fileName, string cloudStorageContainer)
        {
            return await Delete(fileName, cloudStorageContainer);
        }

        public async Task<bool> TryDeleteProductImage(string fileName)
        {
            return await Delete(fileName, "applications-images");
        }
    }
}
