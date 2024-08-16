using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublicationPage
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
        public PublicationCreateViewModel Input { get; set; }

     
        public async Task<IActionResult> OnGetAsync()
        {
            var publicationTermAndCondition = await  _context.Configurations
                .Where(x => x.Key == ConfigurationConstants.TEACHERINVESTIGATION.PUBLICATION_TERMS_AND_CONDITION)
                .FirstOrDefaultAsync();


            var termsCondition = "";

            if (publicationTermAndCondition != null)
                termsCondition = publicationTermAndCondition.Value;

            ViewData["TermsAndCondition"] = termsCondition;

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var storage = new CloudStorageService(_storageCredentials);

            var user = await _userManager.GetUserAsync(User);

            if (!Input.TermnConditions)
                return new BadRequestObjectResult("Debe aceptar los términos y condiciones");

            var publication = new Publication
                {
                    AuthorshipOrderId = Input.AuthorShipOrderId,
                    OpusTypeId = Input.OpusTypeId.Value,
                    PublicationFunctionId = Input.PublicationFunctionId,
                    IndexPlaceId = Input.IndexPlaceId,
                    IdentificationTypeId = Input.IdentificationTypeId,
                    WorkCategory = Input.WorkCategory,
                    Title = Input.Title,
                    SubTitle = Input.SubTitle,
                    Journal = Input.Journal,
                    Description = Input.Description,
                    Volume = Input.Volume,
                    Fascicle = Input.Fascicle,
                    MainAuthor = Input.MainAuthor,
                    PublishingHouse = Input.PublishingHouse,
                    DOI = Input.DOI,
                    PublishDate = ConvertHelpers.DatepickerToUtcDateTime(Input.PublishDate),
                    UserId = user.Id
                };
                await _context.Publications.AddAsync(publication);

            if (Input.Authors != null)
            {
                var authorDistinct = Input.Authors.Select(x => x.Dni).Distinct().ToList();
                var authorTotal = Input.Authors.Select(x => x.Dni).ToList();

                if (authorDistinct.Count != authorTotal.Count)
                {
                    return new BadRequestObjectResult("Existe al menos un autor con DNI repetido");
                }

                foreach (var authorArr in Input.Authors)
                {
                    var author = new PublicationAuthor
                    {
                        PublicationId = publication.Id,
                        PaternalSurname = authorArr.PaternalSurname,
                        MaternalSurname = authorArr.MaternalSurname,
                        Name = authorArr.Name,
                        Email = authorArr.Email,
                        Dni = authorArr.Dni
                    };

                    await _context.PublicationAuthors.AddAsync(author);
                }
            }

            if (Input.PublicationFiles != null)
            {
                foreach (var fileArr in Input.PublicationFiles)
                {
                    string fileUrl = await storage.UploadFile(fileArr.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLICATION_DOCUMENTS,
                        Path.GetExtension(fileArr.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                    var file = new PublicationFile
                    {
                        PublicationId = publication.Id,
                        Name = fileArr.Name,
                        FilePath = fileUrl
                    };

                    await _context.PublicationFiles.AddAsync(file);

                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
