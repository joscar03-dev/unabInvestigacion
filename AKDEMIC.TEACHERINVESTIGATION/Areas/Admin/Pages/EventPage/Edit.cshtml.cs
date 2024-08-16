using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.EventPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class EditModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EditModel(
        IOptions<CloudStorageCredentials> storageCredentials,
        AkdemicContext context,
        IDataTablesService dataTablesService
        )
        {
            _storageCredentials = storageCredentials;
            _context = context;
            _dataTablesService = dataTablesService;
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
                UnitId = e.UnitId,
                PicturePath = e.PicturePath
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var e = await _context.Events.Where(x => x.Id == Input.Id).FirstOrDefaultAsync();

            if (e == null)
                return new BadRequestObjectResult("Sucedio un error");

            
            e.Title = Input.Title;
            e.Description = Input.Description;
            e.EventDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EventDate);
            e.VideoUrl = Input.VideoUrl;
            e.Cost = Input.Cost;
            e.Organizer = Input.Organizer;
            e.UnitId = Input.UnitId;
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
