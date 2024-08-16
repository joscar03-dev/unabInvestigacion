using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.FacultyViewModels;
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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT)]
    public class EditConvocationModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IAsyncRepository<IncubatorConvocation> _incubatorConvocationRepository;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditConvocationModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IAsyncRepository<IncubatorConvocation> incubatorConvocationRepository,
            IOptions<CloudStorageCredentials> storageCredentials)
        {
            _incubatorConvocationRepository = incubatorConvocationRepository;
            _storageCredentials = storageCredentials;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public IncubatorConvocationEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorConvocationId)
        {
            var incubatorConvocation = await _context.IncubatorConvocations
                .Where(x => x.Id == incubatorConvocationId)
                .Select(x => new IncubatorConvocationEditViewModel
                {
                    IncubatorConvocationId = x.Id,
                    Name = x.Name,
                    Code = x.Code,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    AddresedTo = x.AddressedTo,
                    Requirements = x.Requirements,
                    PicturePath = x.PicturePath,
                    DocumentPath = x.DocumentPath,
                    InscriptionStartDate = x.InscriptionStartDate.ToLocalDateTimeFormat(),
                    InscriptionEndDate = x.InscriptionEndDate.ToLocalDateTimeFormat(),
                    TotalWinners = x.TotalWinners,
                    Faculties = x.IncubatorConvocationFaculties
                        .Select(y => new FacultyViewModel 
                        {
                            id = y.FacultyId,
                            name = y.FacultyText
                        })
                        .ToList(),
                })
                .FirstOrDefaultAsync();

            if (incubatorConvocation == null)
                return RedirectToPage("Index");

            Input = incubatorConvocation;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var incubatorConvocation = await _incubatorConvocationRepository.GetByIdAsync(Input.IncubatorConvocationId);

            if (incubatorConvocation == null)
                return new BadRequestObjectResult("Sucedio un error");


            var storage = new CloudStorageService(_storageCredentials);

            if (Input.PictureFile != null)
            {
                incubatorConvocation.PicturePath = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORCONVOCATION_PHOTOS,
                         Path.GetExtension(Input.PictureFile.Name), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            if (Input.DocumentFile != null)
            {
                incubatorConvocation.DocumentPath = await storage.UploadFile(Input.DocumentFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORCONVOCATION_DOCUMENTS,
                         Path.GetExtension(Input.DocumentFile.Name), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            incubatorConvocation.Name = Input.Name;
            incubatorConvocation.Code = Input.Code;
            incubatorConvocation.AddressedTo = Input.AddresedTo;
            incubatorConvocation.Requirements = Input.Requirements;
            incubatorConvocation.StartDate = ConvertHelpers.DatepickerToUtcDateTime(Input.StartDate);
            incubatorConvocation.EndDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EndDate);
            incubatorConvocation.InscriptionStartDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InscriptionStartDate);
            incubatorConvocation.InscriptionEndDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InscriptionEndDate);
            incubatorConvocation.TotalWinners = Input.TotalWinners;

            var incubatorConvocationFaculties = await _context.IncubatorConvocationFaculties
                .Where(x => x.IncubatorConvocationId == incubatorConvocation.Id)
                .ToListAsync();

            if (incubatorConvocationFaculties.Count > 0)
            {
                _context.IncubatorConvocationFaculties.RemoveRange(incubatorConvocationFaculties);
            }

            if (Input.Faculties != null)
            {
                for (int i = 0; i < Input.Faculties.Count; i++)
                {
                    var incubatorConvocationFaculty = new IncubatorConvocationFaculty
                    {
                        FacultyId = Input.Faculties[i].id,
                        FacultyText = Input.Faculties[i].name,
                        IncubatorConvocationId = incubatorConvocation.Id
                    };
                    await _context.IncubatorConvocationFaculties.AddAsync(incubatorConvocationFaculty);
                }
            }


            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
