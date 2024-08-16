using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublicationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublicationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class EditModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EditModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _storageCredentials = storageCredentials;
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
        }

        [BindProperty]
        public PublicationEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(Guid publicationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var publication = await _context.Publications
                .Where(x => x.Id == publicationId && x.UserId == user.Id)
                .Select(x => new
                {
                    Id = x.Id,
                    AuthorShipOrderId = x.AuthorshipOrderId,
                    OpusTypeId = x.OpusTypeId,
                    PublicationFunctionId = x.PublicationFunctionId,
                    IndexPlaceId = x.IndexPlaceId,
                    IdentificationTypeId = x.IdentificationTypeId,
                    WorkCategory = x.WorkCategory,
                    Title = x.Title,
                    SubTitle = x.SubTitle,
                    Journal = x.Journal,
                    Description = x.Description,
                    Volume = x.Volume,
                    Fascicle = x.Fascicle,
                    MainAuthor = x.MainAuthor,
                    PublishingHouse = x.PublishingHouse,
                    DOI = x.DOI,
                    PublishDate = x.PublishDate.ToLocalDateFormat()
                }).FirstOrDefaultAsync();

            if (publication == null)
                return RedirectToPage("/Index");

            Input = new PublicationEditViewModel
            {
                Id = publication.Id,
                AuthorShipOrderId = publication.AuthorShipOrderId,
                OpusTypeId = publication.OpusTypeId,
                PublicationFunctionId = publication.PublicationFunctionId,
                IndexPlaceId = publication.IndexPlaceId,
                IdentificationTypeId = publication.IdentificationTypeId,
                WorkCategory = publication.WorkCategory,
                Title = publication.Title,
                SubTitle = publication.SubTitle,
                Journal = publication.Journal,
                Description = publication.Description,
                Volume = publication.Volume,
                Fascicle = publication.Fascicle,
                MainAuthor = publication.MainAuthor,
                PublishingHouse = publication.PublishingHouse,
                DOI = publication.DOI,
                PublishDate = publication.PublishDate
            };

            return Page();
        }
        //Datatable de Autor
        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid publicationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublicationAuthor, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "1":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Email;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
            }

            var query = _context.PublicationAuthors
                .Where(x => x.PublicationId == publicationId && x.Publication.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Email,
                    x.Dni
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }
        //Datatable de Archivo
        public async Task<IActionResult> OnGetFileDatatableAsync(Guid publicationId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var user = await _userManager.GetUserAsync(User);

            Expression<Func<PublicationFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.PublicationFiles
                .Where(x => x.PublicationId == publicationId && x.Publication.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.PublicationId
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var user = await _userManager.GetUserAsync(User);

            var publication = await _context.Publications.Where(x => x.Id == Input.Id && x.UserId == user.Id).FirstOrDefaultAsync();

            if (publication == null)
                return new BadRequestObjectResult("Sucedio un error");


            publication.AuthorshipOrderId = Input.AuthorShipOrderId;
            publication.OpusTypeId = Input.OpusTypeId.Value;
            publication.PublicationFunctionId = Input.PublicationFunctionId;
            publication.IndexPlaceId = Input.IndexPlaceId;
            publication.IdentificationTypeId = Input.IdentificationTypeId;
            publication.WorkCategory = Input.WorkCategory;
            publication.Title = Input.Title;
            publication.SubTitle = Input.SubTitle;
            publication.Journal = Input.Journal;
            publication.Description = Input.Description;
            publication.Volume = Input.Volume;
            publication.Fascicle = Input.Fascicle;
            publication.MainAuthor = Input.MainAuthor;
            publication.PublishingHouse = Input.PublishingHouse;
            publication.DOI = Input.DOI;
            publication.PublishDate = ConvertHelpers.DatepickerToUtcDateTime(Input.PublishDate);

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #region CRUD AUTHOR
        public async Task<IActionResult> OnPostCreatePublicationAuthorAsync(PublicationAuthorCreateViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            if (await _context.PublicationAuthors.Where(x => x.PublicationId == viewModel.PublicationId).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            var publicatonAuthor = new PublicationAuthor
            {
                PublicationId = viewModel.PublicationId,
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Name = viewModel.Name,
                Email = viewModel.Email,
                Dni = viewModel.Dni
            };

            await _context.PublicationAuthors.AddAsync(publicatonAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDetailPublicationAuthorAsync(Guid id)
        {
            var publicationAuthor = await _context.PublicationAuthors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publicationAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publicationAuthor.PaternalSurname,
                publicationAuthor.MaternalSurname,
                publicationAuthor.Name,
                publicationAuthor.Email,
                publicationAuthor.Dni,
                publicationAuthor.Id,
                publicationAuthor.PublicationId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEditPublicationAuthorAsync(PublicationAuthorEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Revise el formulario");
            }

            var user = await _userManager.GetUserAsync(User);

            var publicationAuthor = await _context.PublicationAuthors
                .Where(x => x.Id == viewModel.Id && x.PublicationId == viewModel.PublicationId)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?

            if (publicationAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (await _context.PublicationAuthors.Where(x => x.PublicationId == viewModel.PublicationId && x.Id != viewModel.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            publicationAuthor.PaternalSurname= viewModel.PaternalSurname;
            publicationAuthor.MaternalSurname = viewModel.MaternalSurname;
            publicationAuthor.Name = viewModel.Name;
            publicationAuthor.Email = viewModel.Email;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeletePublicationAuthorAsync(Guid id)
        {
           

            var publicationAuthor = await _context.PublicationAuthors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publicationAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PublicationAuthors.Remove(publicationAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #endregion
        //////////////
        #region CRUD FILE
        public async Task<IActionResult> OnPostCreatePublicationFile(PublicationFileCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");


            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLICATION_DOCUMENTS,
            Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var publicationFile = new PublicationFile
            {
                PublicationId = viewModel.PublicationId,
                Name = viewModel.Name,
                FilePath = fileUrl
            };


            await _context.PublicationFiles.AddAsync(publicationFile);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditPublicationFileAsync(PublicationFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publicationFiles = await _context.PublicationFiles.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (publicationFiles == null) return new BadRequestObjectResult("Sucedio un error");

            publicationFiles.Name = viewModel.Name;
            publicationFiles.FilePath = viewModel.FilePath;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLICATION_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                publicationFiles.FilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailPublicationFileAsync(Guid id)
        {
            var publicationFile = await _context.PublicationFiles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publicationFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publicationFile.Id,
                publicationFile.Name,
                publicationFile.FilePath,
                publicationFile.PublicationId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeletePublicationFileAsync(Guid id)
        {
            var publicationFiles = await _context.PublicationFiles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (publicationFiles == null) return new BadRequestObjectResult("Sucedio un error");

            _context.PublicationFiles.Remove(publicationFiles);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        #endregion
    }
}
