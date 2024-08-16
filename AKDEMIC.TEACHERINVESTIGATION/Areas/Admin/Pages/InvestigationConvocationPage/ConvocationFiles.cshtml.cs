using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class ConvocationFilesModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;
        private readonly IAsyncRepository<InvestigationConvocationFile> _investigationConvocationFileRepository;
        private readonly IInvestigationConvocationFileService _investigationConvocationFileService;
        private readonly IInvestigationConvocationInquiryService _investigationConvocationInquiryService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly AkdemicContext _context;

        public ConvocationFilesModel(
            IDataTablesService dataTablesService,
            IAsyncRepository<InvestigationConvocation> investigationConvocationRepository,
            IAsyncRepository<InvestigationConvocationFile> investigationConvocationFileRepository,
            IInvestigationConvocationFileService investigationConvocationFileService,
            IInvestigationConvocationInquiryService investigationConvocationInquiryService,
            IOptions<CloudStorageCredentials> storageCredentials,
            AkdemicContext context
        )
        {
            _dataTablesService = dataTablesService;
            _investigationConvocationRepository = investigationConvocationRepository;
            _investigationConvocationFileRepository = investigationConvocationFileRepository;
            _investigationConvocationFileService = investigationConvocationFileService;
            _investigationConvocationInquiryService = investigationConvocationInquiryService;
            _storageCredentials = storageCredentials;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Guid InvestigationConvocationId { get; set; }

        public string InvestigationConvocationName { get; set; }

        public string InvestigationConvocationCode { get; set; }

        public string InvestigationConvocationStartDate { get; set; }

        public string InvestigationConvocationEndDate { get; set; }

        public decimal InvestigationConvocationMinScore { get; set; }

        public string InvestigationConvocationState { get; set; }

        public bool InvestigationConvocationAllowInquiries { get; set; }

        public string InvestigationConvocationInquiryStartDate { get; set; }

        public string InvestigationConvocationInquiryEndDate { get; set; }

        /// <summary>
        /// Esta funcion obtiene los archivos de la convocatoria, dentro de una tabla.
        /// </summary>
        /// <param name="investigationConvocationId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetAsync(Guid investigationConvocationId)
        {
            var investigationConvocation = await _investigationConvocationRepository.GetByIdAsync(investigationConvocationId);

            InvestigationConvocationName = investigationConvocation.Name;
            InvestigationConvocationCode = investigationConvocation.Code;
            InvestigationConvocationStartDate = investigationConvocation.StartDate.ToLocalDateFormat();
            InvestigationConvocationEndDate = investigationConvocation.EndDate.ToLocalDateFormat();
            InvestigationConvocationMinScore = investigationConvocation.MinScore;
            InvestigationConvocationState = GeneralConstants.INVESTIGATIONCONVOCATION.STATES.VALUES.ContainsKey(investigationConvocation.State) ? 
                                           GeneralConstants.INVESTIGATIONCONVOCATION.STATES.VALUES[investigationConvocation.State] : "";
            InvestigationConvocationAllowInquiries = investigationConvocation.AllowInquiries;
            InvestigationConvocationInquiryStartDate = investigationConvocation.InquiryStartDate == null ? "" : investigationConvocation.InquiryStartDate.ToLocalDateFormat();
            InvestigationConvocationInquiryEndDate = investigationConvocation.InquiryEndDate == null ? "" : investigationConvocation.InquiryEndDate.ToLocalDateFormat();

            InvestigationConvocationId = investigationConvocationId;
            return Page();
        }
        /// <summary>
        /// Esta funcion muestra la tabla de archivos dentro de la convocatoria.
        /// </summary>
        /// <param name="investigationConvocationId"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDatatableAsync(Guid investigationConvocationId, string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _investigationConvocationFileService.GetInvestigationConvocationFileDatatable(sentParameters, investigationConvocationId, searchValue);

            return new OkObjectResult(result);
        }
        /// <summary>
        /// Esta funcion obtiene si los archivos tiene Consultas dentro la tabla.
        /// </summary>
        /// <param name="investigationConvocationId"></param>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetInquiriesDatatableAsync(Guid investigationConvocationId, string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _investigationConvocationInquiryService.GetInvestigationConvocationInquiryDatatable(sentParameters, investigationConvocationId, searchValue);

            return new OkObjectResult(result);
        }
        /// <summary>
        /// Esta funcion sube los documentos adjuntados dentro del formulario de convocatoria de archivos
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostCreateFileAsync(ConvocationFileViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe subir un archivo");

            var storage = new CloudStorageService(_storageCredentials);


            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var investigationConvocationfile = new InvestigationConvocationFile
            {
                FilePath = fileUrl,
                InvestigationConvocationId = InvestigationConvocationId,
                Name = viewModel.Name,
                Number = viewModel.Number
            };

            await _investigationConvocationFileRepository.InsertAsync(investigationConvocationfile);

            return new OkResult();
        }
        /// <summary>
        ///Esta funcion nos ayuda a editar un objeto,(Puede subir otro documento, o cambiar el nombre del archivo), dentro de los archivos de la convocatoria. 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostEditFileAsync(ConvocationFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigationConvocationFile = await _investigationConvocationFileRepository.GetByIdAsync(viewModel.InvestigationConvocationFileId);

            if (investigationConvocationFile == null)
                return new BadRequestObjectResult("Sucedio un error");


            investigationConvocationFile.Name = viewModel.Name;
            investigationConvocationFile.Number = viewModel.Number;

            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigationConvocationFile.FilePath = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            await _investigationConvocationFileRepository.UpdateAsync(investigationConvocationFile);

            return new OkObjectResult("");
        }
        /// <summary>
        /// Esta funcion de detalle , obtiene los datos de un selecionado objeto dentro de la tabla.
        /// </summary>
        /// <param name="investigationConvocationFileId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDetailFileAsync(Guid investigationConvocationFileId)
        {
            var investigationConvocationFile = await _investigationConvocationFileRepository.GetByIdAsync(investigationConvocationFileId);

            if (investigationConvocationFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new 
            {
                investigationConvocationFile.FilePath,
                investigationConvocationFile.Number,
                investigationConvocationFile.Name,
                investigationConvocationFile.Id
            };
            return new OkObjectResult(result);
        }

        #region EXTERNAL EVALUATORS
        /// <summary>
        /// Esta funcion obtiene la tabla de los evaluadores externos con todas sus caracteristicas.
        /// </summary>
        /// <param name="investigationConvocationId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetExternalEvaluatorDatatableAsync(Guid investigationConvocationId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationConvocationEvaluator, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.InvestigationConvocationEvaluators
                .Where(x => x.InvestigationConvocationId == investigationConvocationId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.UserId,
                    x.InvestigationConvocationId,
                    x.User.UserName,
                    x.User.FullName,
                    x.User.Dni,
                    x.User.Email,
                    x.User.PhoneNumber
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

            return new OkObjectResult(result);
        }
        /// <summary>
        /// Esta funcion envia el formulario para agregar un nuevo evaluador externo.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddExternalEvaluatorAsync(ExternalEvaluatorViewModel model)
        {
            if (await _context.InvestigationConvocationEvaluators.AnyAsync(x => x.InvestigationConvocationId == model.InvestigationConvocationId && x.UserId == model.UserId))
                return BadRequest("El usuario seleccionado ya se encuentra asignado como evaluador");

            var entity = new InvestigationConvocationEvaluator
            {
                InvestigationConvocationId = model.InvestigationConvocationId,
                UserId = model.UserId
            };

            await _context.InvestigationConvocationEvaluators.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        /// <summary>
        /// Esta funcion elimina un objeto de la base de datos. seleccionando a uno de lista.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostDeleteExternalEvaluatorAsync(ExternalEvaluatorViewModel model)
        {
            var entity = await _context.InvestigationConvocationEvaluators.Where(x => x.InvestigationConvocationId == model.InvestigationConvocationId && x.UserId == model.UserId).FirstOrDefaultAsync();

            if (entity is null)
                return BadRequest("No se encontr? al evaluador seleccionado.");

            _context.InvestigationConvocationEvaluators.Remove(entity);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion
    }
}
