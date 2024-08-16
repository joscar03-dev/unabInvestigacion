using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.Pages.EventPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT + "," +
    GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT + "," +
    GeneralConstants.ROLES.PUBLICATION_UNIT + "," +
    GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT)]
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
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var unit = await _context.Units.Where(x => x.UserId == user.Id).FirstOrDefaultAsync();

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
                UnitId = unit.Id,
            };

            await _context.Events.AddAsync(e);
            await _context.SaveChangesAsync();


            return new OkResult();
        }
    }
}
