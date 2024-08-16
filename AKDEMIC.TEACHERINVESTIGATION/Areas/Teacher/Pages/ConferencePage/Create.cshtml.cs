using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.ConferenceViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.ConferencePage
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
        public ConferenceCreateViewModel Input { get; set; }

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

            if (string.IsNullOrEmpty(Input.StartDate) || string.IsNullOrEmpty(Input.EndDate))
                return new BadRequestObjectResult("Debe especificar la fecha de inicio y fecha de fin");

            if (string.IsNullOrEmpty(Input.Title) || string.IsNullOrEmpty(Input.Name))
                return new BadRequestObjectResult("Debe especificar el título y nombre del congreso");


            DateTime startDate = ConvertHelpers.DatepickerToUtcDateTime(Input.StartDate);
            DateTime endDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EndDate);

            if (endDate < startDate)
                return new BadRequestObjectResult("La fecha de fin no puede ser menor a la fecha de inicio");


            var conference = new Conference
            {
                Type = Input.Type,
                Title = Input.Title,
                Name = Input.Name,
                OrganizerInstitution = Input.OrganizerInstitution,
                Country = Input.Country,
                City = Input.City,
                StartDate = startDate,
                EndDate = endDate,
                MainAuthor = Input.MainAuthor,
                ISBN = Input.ISBN,
                ISSN = Input.ISSN,
                DOI = Input.DOI,
                UserId = user.Id,
                UrlEvent = Input.UrlEvent
            };

            if (Input.OpusTypeId != null)
                conference.OpusTypeId = Input.OpusTypeId.Value;

            await _context.Conferences.AddAsync(conference);

            if (Input.ConferenceAuthors != null)
            {
                var authorDistinct = Input.ConferenceAuthors.Select(x => x.Dni).Distinct().ToList();
                var authorTotal = Input.ConferenceAuthors.Select(x => x.Dni).ToList();

                if (authorDistinct.Count != authorTotal.Count)
                {
                    return new BadRequestObjectResult("Existe al menos un autor con DNI repetido");
                }

                foreach (var authorArr in Input.ConferenceAuthors)
                {
                    var author = new ConferenceAuthor
                    {
                        ConferenceId = conference.Id,
                        PaternalSurname = authorArr.PaternalSurname,
                        MaternalSurname = authorArr.MaternalSurname,
                        Name = authorArr.Name,
                        Email = authorArr.Email,
                        Dni = authorArr.Dni
                    };

                    await _context.ConferenceAuthors.AddAsync(author);
                }
            }

            if (Input.ConferenceFiles != null)
            {
                foreach (var fileArr in Input.ConferenceFiles)
                {
                    string fileUrl = await storage.UploadFile(fileArr.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.CONFERENCE_DOCUMENTS,
                        Path.GetExtension(fileArr.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                    var file = new ConferenceFile
                    {
                        ConferenceId = conference.Id,
                        Name = fileArr.Name,
                        FilePath = fileUrl
                    };

                    await _context.ConferenceFiles.AddAsync(file);
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
