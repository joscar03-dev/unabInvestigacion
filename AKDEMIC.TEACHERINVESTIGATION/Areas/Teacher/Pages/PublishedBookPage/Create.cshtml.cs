using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedBookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublishedBookPage
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
        public PublishedBookCreateViewModel Input { get; set; }

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

            if (string.IsNullOrEmpty(Input.Title))
                return new BadRequestObjectResult("Debe especificar el título del libro publicado");

            var publishedBook = new PublishedBook
            {
                Title = Input.Title,
                PublishingCity = Input.PublishingCity,
                PublishingHouse = Input.PublishingHouse,
                PublishingYear = Input.PublishingYear,
                PagesCount = Input.PagesCount,
                MainAuthor = Input.MainAuthor,
                ISBN = Input.ISBN,
                LegalDeposit = Input.LegalDeposit,
                UserId = user.Id,
                Url = Input.Url
            };

            await _context.PublishedBooks.AddAsync(publishedBook);

            if (Input.PublishedBookAuthors != null)
            {
                var authorDistinct = Input.PublishedBookAuthors.Select(x => x.Dni).Distinct().ToList();
                var authorTotal = Input.PublishedBookAuthors.Select(x => x.Dni).ToList();

                if (authorDistinct.Count != authorTotal.Count)
                {
                    return new BadRequestObjectResult("Existe al menos un autor con DNI repetido");
                }

                foreach (var authorArr in Input.PublishedBookAuthors)
                {
                    var author = new PublishedBookAuthor
                    {
                        PublishedBookId = publishedBook.Id,
                        PaternalSurname = authorArr.PaternalSurname,
                        MaternalSurname = authorArr.MaternalSurname,
                        Name = authorArr.Name,
                        Email = authorArr.Email,
                        Dni = authorArr.Dni,
                    };

                    await _context.PublishedBookAuthors.AddAsync(author);
                }
            }

            if (Input.PublishedBookFiles != null)
            {
                foreach (var fileArr in Input.PublishedBookFiles)
                {
                    string fileUrl = await storage.UploadFile(fileArr.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLISHEDBOOK_DOCUMENTS,
                        Path.GetExtension(fileArr.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                    var file = new PublishedBookFile
                    {
                        PublishedBookId = publishedBook.Id,
                        Name = fileArr.Name,
                        FilePath = fileUrl
                    };

                    await _context.PublishedBookFiles.AddAsync(file);
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
