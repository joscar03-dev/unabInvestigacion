using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT)]
    public class ConvocationFilesModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IAsyncRepository<IncubatorConvocation> _incubatorConvocationRepository;
        private readonly IAsyncRepository<IncubatorConvocationFile> _incubatorConvocationFileRepository;
        private readonly IAsyncRepository<IncubatorConvocationAnnex> _incubatorConvocationAnnexRepository;
        private readonly IIncubatorConvocationFileService _incubatorConvocationFileService;
        private readonly IIncubatorConvocationAnnexService _incubatorConvocationAnnexService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly AkdemicContext _context;

        public ConvocationFilesModel(
            IDataTablesService dataTablesService,
            IAsyncRepository<IncubatorConvocation> incubatorConvocationRepository,
            IAsyncRepository<IncubatorConvocationFile> incubatorConvocationFileRepository,
            IAsyncRepository<IncubatorConvocationAnnex> incubatorConvocationAnnexRepository,
            IIncubatorConvocationFileService incubatorConvocationFileService,
            IIncubatorConvocationAnnexService incubatorConvocationAnnexService,
            IOptions<CloudStorageCredentials> storageCredentials,
            AkdemicContext context
        )
        {
            _dataTablesService = dataTablesService;
            _incubatorConvocationRepository = incubatorConvocationRepository;
            _incubatorConvocationFileRepository = incubatorConvocationFileRepository;
            _incubatorConvocationAnnexRepository = incubatorConvocationAnnexRepository;
            _incubatorConvocationFileService = incubatorConvocationFileService;
            _incubatorConvocationAnnexService = incubatorConvocationAnnexService;
            _storageCredentials = storageCredentials;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Guid IncubatorConvocationId { get; set; }

        public string IncubatorConvocationName { get; set; }

        public string IncubatorConvocationCode { get; set; }

        public string IncubatorConvocationStartDate { get; set; }

        public string IncubatorConvocationEndDate { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorConvocationId)
        {
            var incubatorConvocation = await _incubatorConvocationRepository.GetByIdAsync(incubatorConvocationId);

            IncubatorConvocationName = incubatorConvocation.Name;
            IncubatorConvocationCode = incubatorConvocation.Code;
            IncubatorConvocationStartDate = incubatorConvocation.StartDate.ToLocalDateFormat();
            IncubatorConvocationEndDate = incubatorConvocation.EndDate.ToLocalDateFormat();
            

            IncubatorConvocationId = IncubatorConvocationId;
            return Page();
        }

        public async Task<IActionResult> OnGetDatatableAsync(Guid incubatorConvocationId, string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _incubatorConvocationFileService.GetIncubatorConvocationFileDatatable(sentParameters, incubatorConvocationId, searchValue);

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostCreateFileAsync(IncubatorConvocationFileCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe subir un archivo");

            var storage = new CloudStorageService(_storageCredentials);


            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORCONVOCATION_DOCUMENTS,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var incubatorConvocationFile = new IncubatorConvocationFile
            {
                FilePath = fileUrl,
                IncubatorConvocationId = IncubatorConvocationId,
                Name = viewModel.Name,
                Number = viewModel.Number
            };

            await _incubatorConvocationFileRepository.InsertAsync(incubatorConvocationFile);

            return new OkResult();
        }
        /// <summary>
        ///Esta funcion nos ayuda a editar un objeto,(Puede subir otro documento, o cambiar el nombre del archivo), dentro de los archivos de la convocatoria. 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostEditFileAsync(IncubatorConvocationFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var incubatorConvocationFile = await _incubatorConvocationFileRepository.GetByIdAsync(viewModel.IncubatorConvocationFileId);

            if (incubatorConvocationFile == null)
                return new BadRequestObjectResult("Sucedio un error");


            incubatorConvocationFile.Name = viewModel.Name;
            incubatorConvocationFile.Number = viewModel.Number;

            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                incubatorConvocationFile.FilePath = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            await _incubatorConvocationFileRepository.UpdateAsync(incubatorConvocationFile);

            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnGetDetailFileAsync(Guid incubatorConvocationFileId)
        {
            var incubatorConvocationFile = await _incubatorConvocationFileRepository.GetByIdAsync(incubatorConvocationFileId);

            if (incubatorConvocationFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                incubatorConvocationFile.FilePath,
                incubatorConvocationFile.Number,
                incubatorConvocationFile.Name,
                incubatorConvocationFile.Id
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteIncubatorConvocationFileAsync(Guid Id)
        {
            var incubatorConvocationFile  = await _context.IncubatorConvocationFiles.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (incubatorConvocationFile is null)
                return BadRequest("No se encontró al archivo seleccionado");

            _context.IncubatorConvocationFiles.Remove(incubatorConvocationFile);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        //

        public async Task<IActionResult> OnGetDatatableAnnexAsync(Guid incubatorConvocationId, string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _incubatorConvocationAnnexService.GetIncubatorConvocationAnnexDatatable(sentParameters, incubatorConvocationId, searchValue);

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostCreateAnnexAsync(IncubatorConvocationAnnexCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var incubatorConvocationAnnex = new IncubatorConvocationAnnex
            {
                
                IncubatorConvocationId = IncubatorConvocationId,
                Name = viewModel.Name,
                Code = viewModel.Code
            };

            await _incubatorConvocationAnnexRepository.InsertAsync(incubatorConvocationAnnex);

            return new OkResult();
        }
        /// <summary>
        ///Esta funcion nos ayuda a editar un objeto,(Puede subir otro documento, o cambiar el nombre del archivo), dentro de los archivos de la convocatoria. 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostEditAnnexAsync(IncubatorConvocationAnnexEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var incubatorConvocationAnnex = await _incubatorConvocationAnnexRepository.GetByIdAsync(viewModel.IncubatorConvocationAnnexId);

            incubatorConvocationAnnex.Name = viewModel.Name;
            incubatorConvocationAnnex.Code = viewModel.Code;

            await _incubatorConvocationAnnexRepository.UpdateAsync(incubatorConvocationAnnex);

            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnGetDetailAnnexAsync(Guid incubatorConvocationAnnexId)
        {
            var incubatorConvocationAnnex = await _incubatorConvocationAnnexRepository.GetByIdAsync(incubatorConvocationAnnexId);

            if (incubatorConvocationAnnex == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                
                incubatorConvocationAnnex.Name,
                incubatorConvocationAnnex.Code,
                incubatorConvocationAnnex.Id
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteIncubatorConvocationAnnexAsync(Guid Id)
        {
            var incubatorConvocationAnnex = await _context.IncubatorConvocationAnnexes.Where(x => x.Id == Id).FirstOrDefaultAsync();

            if (incubatorConvocationAnnex is null)
                return BadRequest("No se encontró al archivo seleccionado");

            _context.IncubatorConvocationAnnexes.Remove(incubatorConvocationAnnex);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteExternalEvaluatorAsync(ExternalEvaluatorViewModel model)
        {
            var entity = await _context.IncubatorConvocationEvaluators.Where(x => x.IncubatorConvocationId == model.IncubatorConvocationId && x.UserId == model.UserId).FirstOrDefaultAsync();

            if (entity == null)
                return BadRequest("No se encontró al evaluador seleccionado.");

            _context.IncubatorConvocationEvaluators.Remove(entity);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostAddExternalEvaluatorAsync(ExternalEvaluatorViewModel model)
        {
            if (await _context.IncubatorConvocationEvaluators.AnyAsync(x => x.IncubatorConvocationId == model.IncubatorConvocationId && x.UserId == model.UserId))
                return BadRequest("El usuario seleccionado ya se encuentra asignado como evaluador");

            var incubatorConvocationEvaluator = new IncubatorConvocationEvaluator
            {
                IncubatorConvocationId = model.IncubatorConvocationId,
                UserId = model.UserId
            };

            await _context.IncubatorConvocationEvaluators.AddAsync(incubatorConvocationEvaluator);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetExternalEvaluatorDatatableAsync(Guid incubatorConvocationId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorConvocationEvaluator, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.IncubatorConvocationEvaluators
                .Where(x => x.IncubatorConvocationId == incubatorConvocationId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.UserId,
                    x.IncubatorConvocationId,
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
    }
}
