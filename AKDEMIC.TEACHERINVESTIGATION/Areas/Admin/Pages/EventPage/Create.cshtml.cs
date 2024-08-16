using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.EventPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
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
        public EventCreateViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var storage = new CloudStorageService(_storageCredentials);

            

            string pictureUrl = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.EVENTS,
                    Path.GetExtension(Input.PictureFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var e = new Event
            {
                Title = Input.Title,
                Description = Input.Description,
                EventDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EventDate),
                PicturePath = pictureUrl,
                Cost = Input.Cost,
                Organizer = Input.Organizer,
                UnitId = Input.UnitId,
            };

            await _context.Events.AddAsync(e);
            await _context.SaveChangesAsync();


            return new OkResult();
        }
    }
}
