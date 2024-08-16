using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [AllowAnonymous]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public FileController(
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _storageCredentials = storageCredentials;
        }

        [HttpGet("documentos/{*path}")]
        public async Task DownloadDocument(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var mem = new MemoryStream())
                {
                    var storage = new CloudStorageService(_storageCredentials);

                    await storage.TryDownload(mem, "", path);

                    // Download file
                    var fileName = Path.GetFileName(path);
                    var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                    HttpContext.Response.Headers["Content-Disposition"] = text;
                    mem.Position = 0;
                    await mem.CopyToAsync(HttpContext.Response.Body);
                }
            }
        }
        [HttpGet("imagenes/{*path}")]
        public async Task DownloadImage(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var mem = new MemoryStream())
                {
                    var storage = new CloudStorageService(_storageCredentials);

                    await storage.TryDownload(mem, "", path);

                    // Download file
                    var fileName = Path.GetFileName(path);
                    var text = $"inline;filename=\"{fileName.Normalize().Replace(' ', '_')}\"";
                    HttpContext.Response.Headers["Content-Disposition"] = text;
                    mem.Position = 0;
                    await mem.CopyToAsync(HttpContext.Response.Body);
                }
            }

        }
    }
}
