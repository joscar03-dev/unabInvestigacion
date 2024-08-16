using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedChapterBookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublishedChapterBookPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class CreateModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public CreateModel(
            IOptions<CloudStorageCredentials> storageCredentials,
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager
        )
        {
            _storageCredentials = storageCredentials;
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;

        }

        [BindProperty]
        public PublishedChapterBookCreateViewModel Input { get; set; }

        public void OnGet()
        {
            ViewData["TermsAndCondition"] = "Acepto que la información proporcionada es verídica.";
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var storage = new CloudStorageService(_storageCredentials);

            var user = await _userManager.GetUserAsync(User);

            if (!Input.TermnConditions)
                return new BadRequestObjectResult("Debe aceptar los términos y condiciones");

            if (string.IsNullOrEmpty(Input.BookTitle) || string.IsNullOrEmpty(Input.ChapterTitle))
                return new BadRequestObjectResult("Debe especificar el título del libro y el título del capítulo");

            var publishedChapterBook = new PublishedChapterBook
            {
                BookTitle = Input.BookTitle,
                ChapterTitle = Input.ChapterTitle,
                PublishingCity = Input.PublishingCity,
                PublishingHouse = Input.PublishingHouse,
                PublishingYear = Input.PublishingYear,
                StartPage = Input.StartPage,
                EndPage = Input.EndPage,
                MainAuthor = Input.MainAuthor,
                ISBN = Input.ISBN,
                DOI = Input.DOI,
                UserId = user.Id,
                Url = Input.Url,
            };

            await _context.PublishedChapterBooks.AddAsync(publishedChapterBook);

            if (Input.PublishedChapterBookAuthors != null)
            {
                var authorDistinct = Input.PublishedChapterBookAuthors.Select(x => x.Dni).Distinct().ToList();
                var authorTotal = Input.PublishedChapterBookAuthors.Select(x => x.Dni).ToList();

                if (authorDistinct.Count != authorTotal.Count)
                {
                    return new BadRequestObjectResult("Existe al menos un autor con DNI repetido");
                }

                foreach (var authorArr in Input.PublishedChapterBookAuthors)
                {
                    var author = new PublishedChapterBookAuthor
                    {
                        PublishedChapterBookId = publishedChapterBook.Id,
                        PaternalSurname = authorArr.PaternalSurname,
                        MaternalSurname = authorArr.MaternalSurname,
                        Name = authorArr.Name,
                        Email = authorArr.Email,
                        Dni = authorArr.Dni,
                    };

                    await _context.PublishedChapterBookAuthors.AddAsync(author);
                }
            }

            if (Input.PublishedChapterBookFiles != null)
            {
                foreach (var fileArr in Input.PublishedChapterBookFiles)
                {
                    string fileUrl = await storage.UploadFile(fileArr.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLISHEDCHAPTERBOOK_DOCUMENTS,
                        Path.GetExtension(fileArr.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                    var file = new PublishedChapterBookFile
                    {
                        PublishedChapterBookId = publishedChapterBook.Id,
                        Name = fileArr.Name,
                        FilePath = fileUrl
                    };

                    await _context.PublishedChapterBookFiles.AddAsync(file);
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
