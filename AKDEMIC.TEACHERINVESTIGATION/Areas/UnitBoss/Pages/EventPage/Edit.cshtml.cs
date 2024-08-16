using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.Pages.EventPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT + "," +
    GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT + "," +
    GeneralConstants.ROLES.PUBLICATION_UNIT + "," +
    GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT)]

    public class EditModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EditModel(
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

        [BindProperty(SupportsGet = true)]
        public EventEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid eventId)
        {
            var e = await _context.Events.Where(x => x.Id == eventId)
                            .Select(x => new
                            {
                                Id = x.Id,
                                Title = x.Title,
                                Description = x.Description,
                                EventDate = x.EventDate.ToLocalDateFormat(),
                                VideoUrl = x.VideoUrl,
                                Cost = x.Cost,
                                Organizer = x.Organizer,
                                UnitId = x.UnitId,
                                PicturePath = x.PicturePath
                            }).FirstOrDefaultAsync();

            if (e == null)
                return RedirectToPage("/Index");

            Input = new EventEditViewModel
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                EventDate = e.EventDate,
                VideoUrl = e.VideoUrl,
                Cost = e.Cost,
                Organizer = e.Organizer,
                PicturePath = e.PicturePath
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var unit = await _context.Units.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

            var e = await _context.Events.Where(x => x.Id == Input.Id).FirstOrDefaultAsync();

            if (e == null)
                return new BadRequestObjectResult("Sucedio un error");


            e.Title = Input.Title;
            e.Description = Input.Description;
            e.EventDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EventDate);
            e.VideoUrl = Input.VideoUrl;
            e.Cost = Input.Cost;
            e.Organizer = Input.Organizer;
            e.UnitId = unit.Id;
            e.PicturePath = Input.PicturePath;

            var storage = new CloudStorageService(_storageCredentials);

            if (Input.PictureFile != null)
            {
                string fileUrl = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PUBLICATION_DOCUMENTS,
                Path.GetExtension(Input.PictureFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                e.PicturePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
