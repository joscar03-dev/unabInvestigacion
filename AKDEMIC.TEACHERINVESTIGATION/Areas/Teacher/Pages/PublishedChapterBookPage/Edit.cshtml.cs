using AKDEMIC.CORE.Constants;
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
using System.Threading.Tasks;
using System;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedChapterBookViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;
using System.IO;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublishedChapterBookPage
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
        public PublishedChapterBookEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid publishedChapterBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedChapterBook = await _context.PublishedChapterBooks
                .Where(x => x.Id == publishedChapterBookId && x.UserId == user.Id)
                .Select(x => new
                {
                    Id = x.Id,
                    BookTitle = x.BookTitle,
                    ChapterTitle = x.ChapterTitle,
                    PublishingHouse = x.PublishingHouse,
                    PublishingCity = x.PublishingCity,
                    PublishingYear = x.PublishingYear,
                    StartPage = x.StartPage,
                    EndPage = x.EndPage,
                    DOI = x.DOI,
                    Url = x.Url,
                    MainAuthor = x.MainAuthor,
                    ISBN = x.ISBN,
                }).FirstOrDefaultAsync();

            if (publishedChapterBook == null)
                return RedirectToPage("/Index");

            Input = new PublishedChapterBookEditViewModel
            {
                Id = publishedChapterBook.Id,
                BookTitle = publishedChapterBook.BookTitle,
                ChapterTitle = publishedChapterBook.ChapterTitle,
                PublishingHouse = publishedChapterBook.PublishingHouse,
                PublishingCity = publishedChapterBook.PublishingCity,
                PublishingYear = publishedChapterBook.PublishingYear,
                StartPage = publishedChapterBook.StartPage,
                EndPage = publishedChapterBook.EndPage,
                DOI = publishedChapterBook.DOI,
                Url = publishedChapterBook.Url,
                MainAuthor = publishedChapterBook.MainAuthor,
                ISBN = publishedChapterBook.ISBN
            };

            return Page();
        }

        #region Author

        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid publishedChapterBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedChapterBookAuthor, dynamic>> orderByPredicate = null;
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

            var query = _context.PublishedChapterBookAuthors
                .Where(x => x.PublishedChapterBookId == publishedChapterBookId && x.PublishedChapterBook.UserId == user.Id)
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

        public async Task<IActionResult> OnPostCreateAuthorAsync(PublishedChapterBookAuthorCreateViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedChapterBook = await _context.PublishedChapterBooks.Where(x => x.Id == viewModel.PublishedChapterBookId && x.UserId == user.Id).FirstOrDefaultAsync();

            if (publishedChapterBook == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (await _context.PublishedChapterBookAuthors.Where(x => x.PublishedChapterBookId == publishedChapterBook.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            var pñublishedChapterBookAuthor = new PublishedChapterBookAuthor
            {
                PublishedChapterBookId = publishedChapterBook.Id,
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Name = viewModel.Name,
                Email = viewModel.Email,
                Dni = viewModel.Dni,
            };

            await _context.PublishedChapterBookAuthors.AddAsync(pñublishedChapterBookAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDetailAuthorAsync(Guid id)
        {
            var publishedChapterBookAuthor = await _context.PublishedChapterBookAuthors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publishedChapterBookAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publishedChapterBookAuthor.PaternalSurname,
                publishedChapterBookAuthor.MaternalSurname,
                publishedChapterBookAuthor.Name,
                publishedChapterBookAuthor.Email,
                publishedChapterBookAuthor.Dni,
                publishedChapterBookAuthor.Id,
                publishedChapterBookAuthor.PublishedChapterBookId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEditAuthorAsync(PublishedChapterBookAuthorEditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedChapterBookAuthor = await _context.PublishedChapterBookAuthors
                .Where(x => x.Id == viewModel.Id && x.PublishedChapterBookId == viewModel.PublishedChapterBookId && x.PublishedChapterBook.UserId == user.Id)
                .FirstOrDefaultAsync();

            //validacion de ser el usuario del proyecto o miembro del equipo?
            if (publishedChapterBookAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (await _context.PublishedChapterBookAuthors.Where(x => x.PublishedChapterBookId == viewModel.PublishedChapterBookId && x.Id != viewModel.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            publishedChapterBookAuthor.PaternalSurname = viewModel.PaternalSurname;
            publishedChapterBookAuthor.MaternalSurname = viewModel.MaternalSurname;
            publishedChapterBookAuthor.Name = viewModel.Name;
            publishedChapterBookAuthor.Email = viewModel.Email;
            publishedChapterBookAuthor.Dni = viewModel.Dni;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAuthorAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var publishedChapterBookAuthor = await _context.PublishedChapterBookAuthors
                .Where(x => x.Id == id && x.PublishedChapterBook.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (publishedChapterBookAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.PublishedChapterBookAuthors.Remove(publishedChapterBookAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }


        #endregion

        #region File

        public async Task<IActionResult> OnGetFileDatatableAsync(Guid publishedChapterBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedChapterBookFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.PublishedChapterBookFiles
                .Where(x => x.PublishedChapterBookId == publishedChapterBookId && x.PublishedChapterBook.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.PublishedChapterBookId
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

        public async Task<IActionResult> OnPostCreateFile(PublishedChapterBookFileCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publishedChapterBook = await _context.PublishedChapterBooks.Where(x => x.Id == viewModel.PublishedChapterBookId).FirstOrDefaultAsync();

            if (publishedChapterBook == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLISHEDCHAPTERBOOK_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var publishedChapterBookFile = new PublishedChapterBookFile
            {
                PublishedChapterBookId = publishedChapterBook.Id,
                Name = viewModel.Name,
                FilePath = fileUrl
            };

            await _context.PublishedChapterBookFiles.AddAsync(publishedChapterBookFile);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditFileAsync(PublishedChapterBookFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publishedChapterBookFile = await _context.PublishedChapterBookFiles.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (publishedChapterBookFile == null) return new BadRequestObjectResult("Sucedio un error");

            publishedChapterBookFile.Name = viewModel.Name;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLISHEDCHAPTERBOOK_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                publishedChapterBookFile.FilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailFileAsync(Guid id)
        {
            var publishedChapterBookFile = await _context.PublishedChapterBookFiles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (publishedChapterBookFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publishedChapterBookFile.Id,
                publishedChapterBookFile.Name,
                publishedChapterBookFile.FilePath,
                publishedChapterBookFile.PublishedChapterBookId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteFileAsync(Guid id)
        {
            var publishedChapterBookFile = await _context.PublishedChapterBookFiles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (publishedChapterBookFile == null) return new BadRequestObjectResult("Sucedio un error");

            _context.PublishedChapterBookFiles.Remove(publishedChapterBookFile);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion



        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var publishedChapterBook = await _context.PublishedChapterBooks.Where(x => x.Id == Input.Id).FirstOrDefaultAsync();

            if (publishedChapterBook == null)
                return new BadRequestObjectResult("Sucedio un error");

            publishedChapterBook.BookTitle = Input.BookTitle;
            publishedChapterBook.ChapterTitle = Input.ChapterTitle;
            publishedChapterBook.PublishingHouse = Input.PublishingHouse;
            publishedChapterBook.PublishingCity = Input.PublishingCity;
            publishedChapterBook.PublishingYear = Input.PublishingYear;
            publishedChapterBook.StartPage = Input.StartPage;
            publishedChapterBook.EndPage = Input.EndPage;
            publishedChapterBook.MainAuthor = Input.MainAuthor;
            publishedChapterBook.ISBN = Input.ISBN;
            publishedChapterBook.DOI = Input.DOI;
            publishedChapterBook.Url = Input.Url;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}

