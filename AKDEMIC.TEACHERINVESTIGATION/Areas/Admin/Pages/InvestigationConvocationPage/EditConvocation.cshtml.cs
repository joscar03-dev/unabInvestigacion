using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.TeacherInvestigation.Implementations;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class EditConvocationModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        protected readonly AkdemicContext _context;
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;
        private readonly IInvestigationConvocationService _investigationConvocationService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly UserManager<ApplicationUser> _userManager;

        public EditConvocationModel(
            AkdemicContext context,
         IDataTablesService dataTablesService,
         IInvestigationConvocationService investigationConvocationService,
         UserManager<ApplicationUser> userManager,
         IAsyncRepository<InvestigationConvocation> investigationConvocationRepository,

       IOptions<CloudStorageCredentials> storageCredentials)
        {
            _dataTablesService = dataTablesService;
            _investigationConvocationService = investigationConvocationService;
            _investigationConvocationRepository = investigationConvocationRepository;
            _storageCredentials = storageCredentials;
            _userManager = userManager;
            _context = context;
        }
      
        [BindProperty(SupportsGet = true)]
        public ConvocationEditViewModel Input { get; set; }

        /// <summary>
        /// Esta Funcion Asincrona captura la data ingresada con el id obtenido para este caso la convocatoria , por medio del get directo de la base de datos.
        /// </summary>
        /// <param name="investigationConvocationId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(Guid investigationConvocationId)
        {
            var investigationConvocation = await _investigationConvocationRepository.GetByIdAsync(investigationConvocationId);

            Input.InvestigationConvocationName = investigationConvocation.Name;
            Input.InvestigationConvocationCode = investigationConvocation.Code;
            Input.InvestigationConvocationStartDate = investigationConvocation.StartDate.ToLocalDateFormat();
            Input.InvestigationConvocationEndDate = investigationConvocation.EndDate.ToLocalDateFormat();
            
            Input.InvestigationConvocationPicturePath = investigationConvocation.PicturePath;
            Input.InvestigationConvocationInscriptionStartDate = investigationConvocation.InscriptionStartDate.ToLocalDateTimeFormat();
            Input.InvestigationConvocationInscriptionEndDate = investigationConvocation.InscriptionEndDate.ToLocalDateTimeFormat();
            Input.InvestigationConvocationDescription = investigationConvocation.Description;
            Input.InvestigationConvocationMinScore = investigationConvocation.MinScore;
            Input.InvestigationConvocationState = GeneralConstants.INVESTIGATIONCONVOCATION.STATES.VALUES.ContainsKey(investigationConvocation.State) ?
                                      GeneralConstants.INVESTIGATIONCONVOCATION.STATES.VALUES[investigationConvocation.State] : "";
            Input.InvestigationConvocationAllowInquiries = investigationConvocation.AllowInquiries;
            Input.InvestigationConvocationInquiryStartDate = investigationConvocation.InquiryStartDate == null ? "" : investigationConvocation.InquiryStartDate.ToLocalDateFormat();
            Input.InvestigationConvocationInquiryEndDate = investigationConvocation.InquiryEndDate == null ? "" : investigationConvocation.InquiryEndDate.ToLocalDateFormat();

            Input.InvestigationConvocationId = investigationConvocationId;
            return Page();
        }

        /// <summary>
        /// Esta Funcion Asincrona captura la data ingresada en el formulario y edita la Convocatoria , por medio del post directo actualiza los parametros en la base de datos.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigationConvocation = await _investigationConvocationRepository.GetByIdAsync(Input.InvestigationConvocationId);

            if (investigationConvocation == null)
                return new BadRequestObjectResult("Sucedio un error");

            DateTime? inquiryStartDate = null;

           
            if (!string.IsNullOrEmpty(Input.InvestigationConvocationInquiryStartDate))
            {
                inquiryStartDate = ConvertHelpers.DatepickerToUtcDateTime(Input.InvestigationConvocationInquiryStartDate);
            }

            DateTime? inquiryEndDate = null;

            if (!string.IsNullOrEmpty(Input.InvestigationConvocationInquiryEndDate))
            {
                inquiryEndDate = ConvertHelpers.DatepickerToUtcDateTime(Input.InvestigationConvocationInquiryEndDate);
            }

            var storage = new CloudStorageService(_storageCredentials);

            if (Input.InvestigationConvocationPictureFile != null)
            {
                investigationConvocation.PicturePath = await storage.UploadFile(Input.InvestigationConvocationPictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_PHOTOS,
                         Path.GetExtension(Input.InvestigationConvocationPictureFile.Name), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            investigationConvocation.Name = Input.InvestigationConvocationName;
            investigationConvocation.Code = Input.InvestigationConvocationCode;
            investigationConvocation.Description = Input.InvestigationConvocationDescription;
            investigationConvocation.StartDate = ConvertHelpers.DatepickerToUtcDateTime(Input.InvestigationConvocationStartDate);
            //investigationConvocation.EndDate = ConvertHelpers.DatepickerToUtcDateTime(Input.InvestigationConvocationEndDate);
            investigationConvocation.InscriptionStartDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InvestigationConvocationInscriptionStartDate);
            investigationConvocation.InscriptionEndDate = ConvertHelpers.DatetimepickerToUtcDateTime(Input.InvestigationConvocationInscriptionEndDate);
            investigationConvocation.MinScore = Input.InvestigationConvocationMinScore;
            investigationConvocation.AllowInquiries = Input.InvestigationConvocationAllowInquiries;
            investigationConvocation.InquiryStartDate = inquiryStartDate;
            investigationConvocation.InquiryEndDate = inquiryEndDate;

          

            await _investigationConvocationRepository.UpdateAsync(investigationConvocation);

            return new OkResult();
        }


        public async Task<IActionResult> OnPostExtensionAsync(Guid investigationConvocationId, ConvocationHistoryCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe subir un archivo");

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return new BadRequestObjectResult("Sucedio un error");

            var investigationConvocation = await _investigationConvocationRepository.GetByIdAsync(Input.InvestigationConvocationId);
            if (investigationConvocation == null)
                return new BadRequestObjectResult("Sucedio un error");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var investigationConvocationHistory = new InvestigationConvocationHistory
            {
                InvestigationConvocationId = investigationConvocationId,
                UserId = user.Id,
                OldEndDate = investigationConvocation.EndDate,
                NewEndDate = ConvertHelpers.DatepickerToUtcDateTime(viewModel.NewEndDate),
                ResolutionUrl = fileUrl
            };

            investigationConvocation.EndDate = ConvertHelpers.DatepickerToUtcDateTime(viewModel.NewEndDate);

            await _context.InvestigationConvocationHistories.AddAsync(investigationConvocationHistory);
            await _context.SaveChangesAsync();

            return new OkResult();


        }

    }
}
