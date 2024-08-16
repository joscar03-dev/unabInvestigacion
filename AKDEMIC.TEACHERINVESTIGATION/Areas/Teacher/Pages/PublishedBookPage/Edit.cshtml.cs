using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedBookViewModels;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;
using System.IO;
using static AKDEMIC.CORE.Constants.Systems.TeacherInvestigationConstants;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublishedBookPage
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
        public PublishedBookEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid publishedBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedBook = await _context.PublishedBooks
                .Where(x => x.Id == publishedBookId && x.UserId == user.Id)
                .Select(x => new
                {
                    Id = x.Id,
                    Title = x.Title,
                    PublishingHouse = x.PublishingHouse,
                    PublishingCity = x.PublishingCity,
                    PublishingYear = x.PublishingYear,
                    PagesCount = x.PagesCount,
                    LegalDeposit = x.LegalDeposit,
                    Url = x.Url,
                    MainAuthor = x.MainAuthor,
                    ISBN = x.ISBN,
                }).FirstOrDefaultAsync();

            if (publishedBook == null)
                return RedirectToPage("/Index");

            Input = new PublishedBookEditViewModel
            {
                Id = publishedBook.Id,
                Title = publishedBook.Title,
                PublishingHouse = publishedBook.PublishingHouse,
                PublishingCity = publishedBook.PublishingCity,
                PublishingYear = publishedBook.PublishingYear,
                PagesCount = publishedBook.PagesCount,
                LegalDeposit = publishedBook.LegalDeposit,
                Url = publishedBook.Url,
                MainAuthor = publishedBook.MainAuthor,
                ISBN = publishedBook.ISBN,
            };

            return Page();
        }

        #region Author

        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid publishedBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedBookAuthor, dynamic>> orderByPredicate = null;
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

            var query = _context.PublishedBookAuthors
                .Where(x => x.PublishedBookId == publishedBookId && x.PublishedBook.UserId == user.Id)
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

        public async Task<IActionResult> OnPostCreateAuthorAsync(PublishedBookAuthorCreateViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedBook = await _context.PublishedBooks.Where(x => x.Id == viewModel.PublishedBookId && x.UserId == user.Id).FirstOrDefaultAsync();

            if (publishedBook == null)
                return new BadRequestObjectResult("Sucedio un error");


            if (await _context.PublishedBookAuthors.Where(x => x.PublishedBookId == publishedBook.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            var publishedBookAuthor = new PublishedBookAuthor
            {
                PublishedBookId = publishedBook.Id,
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Name = viewModel.Name,
                Email = viewModel.Email,
                Dni = viewModel.Dni
            };

            await _context.PublishedBookAuthors.AddAsync(publishedBookAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDetailAuthorAsync(Guid id)
        {
            var publishedBookAuthor = await _context.PublishedBookAuthors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publishedBookAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publishedBookAuthor.PaternalSurname,
                publishedBookAuthor.MaternalSurname,
                publishedBookAuthor.Name,
                publishedBookAuthor.Email,
                publishedBookAuthor.Dni,
                publishedBookAuthor.Id,
                publishedBookAuthor.PublishedBookId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEditAuthorAsync(PublishedBookAuthorEditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedBookAuthor = await _context.PublishedBookAuthors
                .Where(x => x.Id == viewModel.Id && x.PublishedBookId == viewModel.PublishedBookId && x.PublishedBook.UserId == user.Id)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?

            if (publishedBookAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (await _context.PublishedBookAuthors.Where(x => x.PublishedBookId == viewModel.PublishedBookId && x.Id != viewModel.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            publishedBookAuthor.PaternalSurname = viewModel.PaternalSurname;
            publishedBookAuthor.MaternalSurname = viewModel.MaternalSurname;
            publishedBookAuthor.Name = viewModel.Name;
            publishedBookAuthor.Email = viewModel.Email;
            publishedBookAuthor.Dni = viewModel.Dni;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAuthorAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedBookAuthor = await _context.PublishedBookAuthors
                .Where(x => x.Id == id && x.PublishedBook.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (publishedBookAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PublishedBookAuthors.Remove(publishedBookAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }


        #endregion

        #region File

        public async Task<IActionResult> OnGetFileDatatableAsync(Guid publishedBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedBookFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.PublishedBookFiles
                .Where(x => x.PublishedBookId == publishedBookId && x.PublishedBook.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.PublishedBookId
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

        public async Task<IActionResult> OnPostCreateFile(PublishedBookFileCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publishedBook = await _context.PublishedBooks.Where(x => x.Id == viewModel.PublishedBookId).FirstOrDefaultAsync();

            if (publishedBook == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLISHEDBOOK_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var publishedBookFile = new PublishedBookFile
            {
                PublishedBookId = publishedBook.Id,
                Name = viewModel.Name,
                FilePath = fileUrl
            };

            await _context.PublishedBookFiles.AddAsync(publishedBookFile);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditFileAsync(PublishedBookFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publishedBookFile = await _context.PublishedBookFiles.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (publishedBookFile == null) return new BadRequestObjectResult("Sucedio un error");

            publishedBookFile.Name = viewModel.Name;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLISHEDBOOK_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                publishedBookFile.FilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailFileAsync(Guid id)
        {
            var publishedBookFile = await _context.PublishedBookFiles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publishedBookFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publishedBookFile.Id,
                publishedBookFile.Name,
                publishedBookFile.FilePath,
                publishedBookFile.PublishedBookId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteFileAsync(Guid id)
        {
            var publishedBookFile = await _context.PublishedBookFiles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (publishedBookFile == null) return new BadRequestObjectResult("Sucedio un error");

            _context.PublishedBookFiles.Remove(publishedBookFile);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion



        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var publishedBook = await _context.PublishedBooks.Where(x => x.Id == Input.Id).FirstOrDefaultAsync();

            if (publishedBook == null)
                return new BadRequestObjectResult("Sucedio un error");

            publishedBook.Title = Input.Title;
            publishedBook.PublishingHouse = Input.PublishingHouse;
            publishedBook.PublishingCity = Input.PublishingCity;
            publishedBook.PublishingYear = Input.PublishingYear;
            publishedBook.PagesCount = Input.PagesCount;
            publishedBook.MainAuthor = Input.MainAuthor;
            publishedBook.ISBN = Input.ISBN;
            publishedBook.LegalDeposit = Input.LegalDeposit;
            publishedBook.Url = Input.Url;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
