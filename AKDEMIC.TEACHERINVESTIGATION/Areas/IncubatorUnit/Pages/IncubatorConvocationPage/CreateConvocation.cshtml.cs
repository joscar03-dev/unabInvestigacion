using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT)]
    public class CreateConvocationModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IAsyncRepository<IncubatorConvocation> _incubatorConvocationRepository;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public CreateConvocationModel(
            AkdemicContext context,
            IAsyncRepository<IncubatorConvocation> incubatorConvocationRepository,
            IOptions<CloudStorageCredentials> storageCredentials)

        {
            _context = context;
            _incubatorConvocationRepository = incubatorConvocationRepository;
            _storageCredentials = storageCredentials;
        }
        public void OnGet()
        {
        }

        [BindProperty(SupportsGet = true)]
        public IncubatorConvocationCreateViewModel Input { get; set; }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (string.IsNullOrEmpty(Input.StartDate) || string.IsNullOrEmpty(Input.EndDate) || string.IsNullOrEmpty(Input.InscriptionStartDate) || string.IsNullOrEmpty(Input.InscriptionEndDate))
            {
                return new BadRequestObjectResult(" Debe Ingresar la Fecha Inicio y Fecha Fin ");
            }

            var storage = new CloudStorageService(_storageCredentials);

            if (Input.PictureFile == null)
                return new BadRequestObjectResult(" Debe ingresar una imagen ");

            if (Input.DocumentFile == null)
                return new BadRequestObjectResult(" Debe ingresar el documento de la Convocatoria ");


            var incubatorConvocation = new IncubatorConvocation
            {
                Name = Input.Name,
                Code = Input.Code,
                AddressedTo = Input.AddresedTo,
                Requirements = Input.Requirements,
                StartDate = ConvertHelpers.DatepickerToUtcDateTime(Input.StartDate),
                EndDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EndDate),
                InscriptionStartDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InscriptionStartDate),
                InscriptionEndDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InscriptionEndDate),
                PicturePath = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORCONVOCATION_PHOTOS,
                        Path.GetExtension(Input.PictureFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION),
                DocumentPath = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORCONVOCATION_DOCUMENTS,
                        Path.GetExtension(Input.DocumentFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION),
                TotalWinners = Input.TotalWinners
            };

            await _context.IncubatorConvocations.AddAsync(incubatorConvocation);

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

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
