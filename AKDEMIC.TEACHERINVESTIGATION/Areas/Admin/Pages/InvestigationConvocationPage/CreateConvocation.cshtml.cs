using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class CreateConvocationModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IInvestigationConvocationService _investigationConvocationService;
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public CreateConvocationModel(
          IDataTablesService dataTablesService,
          IInvestigationConvocationService investigationConvocationService,
          IAsyncRepository<InvestigationConvocation> investigationConvocationRepository,
          IOptions<CloudStorageCredentials> storageCredentials)
          
        {
            _dataTablesService = dataTablesService;
            _investigationConvocationService = investigationConvocationService;
            _investigationConvocationRepository = investigationConvocationRepository;
            _storageCredentials = storageCredentials;
        }

        public void OnGet()
        {
        }

        [BindProperty(SupportsGet = true)]
        public ConvocationCreateViewModel Input { get; set; }

        /// <summary>
        /// Creacion de una convocatoria en base a los parametros del formulario, se envia mediante un post directo a la base de datos.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (string.IsNullOrEmpty(Input.StartDate)|| string.IsNullOrEmpty(Input.EndDate) || string.IsNullOrEmpty(Input.InscriptionStartDate) || string.IsNullOrEmpty(Input.InscriptionEndDate))
            {
                return new BadRequestObjectResult(" Debe Ingresar la Fecha Inicio y Fecha Fin ");
            }

            DateTime? inquiryStartDate = null;

            if (!string.IsNullOrEmpty(Input.InquiryStartDate))
            {
                inquiryStartDate = ConvertHelpers.DatepickerToUtcDateTime(Input.InquiryStartDate);
            }


            DateTime? inquiryEndDate = null;

            if (!string.IsNullOrEmpty(Input.InquiryEndDate))
            {
                inquiryEndDate = ConvertHelpers.DatepickerToUtcDateTime(Input.InquiryEndDate);
            }

            var storage = new CloudStorageService(_storageCredentials);

            if (Input.PictureFile == null)
            {
                return new BadRequestObjectResult(" Debe ingresar Una Imagen ");
            }
            var investigationConvocation = new InvestigationConvocation
            {
                
                Name = Input.Name,
                Code = Input.Code,
                State = GeneralConstants.INVESTIGATIONCONVOCATION.STATES.OPEN,
                StartDate = ConvertHelpers.DatepickerToUtcDateTime(Input.StartDate),
                EndDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EndDate),
                Description = Input.Description,
                InscriptionStartDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InscriptionStartDate),
                InscriptionEndDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InscriptionEndDate),
                InquiryStartDate = inquiryStartDate,
                InquiryEndDate = inquiryEndDate,
                MinScore = Input.MinScore,
                PicturePath = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_PHOTOS,
                        Path.GetExtension(Input.PictureFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION),
                AllowInquiries = Input.AllowInquiries,
                InvestigationConvocationRequirement = new InvestigationConvocationRequirement()
            };

            await _investigationConvocationRepository.InsertAsync(investigationConvocation);

            return new OkResult();
        }
    }
}
