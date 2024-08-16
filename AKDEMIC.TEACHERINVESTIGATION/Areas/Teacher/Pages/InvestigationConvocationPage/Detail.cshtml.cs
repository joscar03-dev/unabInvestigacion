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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class DetailModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;
        private readonly IAsyncRepository<InvestigationConvocationFile> _investigationConvocationFileRepository;
        private readonly IInvestigationConvocationFileService _investigationConvocationFileService;
        private readonly IInvestigationConvocationInquiryService _investigationConvocationInquiryService;
        private readonly IAsyncRepository<InvestigationConvocationInquiry> _investigationConvocationInquiryRepository;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly AkdemicContext _context;

        public DetailModel(
        IDataTablesService dataTablesService,
        IAsyncRepository<InvestigationConvocation> investigationConvocationRepository,
        IAsyncRepository<InvestigationConvocationFile> investigationConvocationFileRepository,
        IAsyncRepository<InvestigationConvocationInquiry> investigationConvocationInquiryRepository,
        IInvestigationConvocationFileService investigationConvocationFileService,
        IInvestigationConvocationInquiryService investigationConvocationInquiryService,
        IOptions<CloudStorageCredentials> storageCredentials,
        AkdemicContext context
        )
        {
            _dataTablesService = dataTablesService;
            _investigationConvocationInquiryRepository = investigationConvocationInquiryRepository;
            _investigationConvocationRepository = investigationConvocationRepository;
            _investigationConvocationFileRepository = investigationConvocationFileRepository;
            _investigationConvocationFileService = investigationConvocationFileService;
            _investigationConvocationInquiryService = investigationConvocationInquiryService;
            _storageCredentials = storageCredentials;
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Guid InvestigationConvocationPostulantId { get; set; }

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

        public async Task<IActionResult> OnGetAsync(Guid investigationConvocationPostulantId)
        {
            var investigationConvocation = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new
                {
                    x.InvestigationConvocation.Name,
                    x.InvestigationConvocation.Code,
                    StartDate = x.InvestigationConvocation.StartDate.ToLocalDateFormat(),
                    EndDate = x.InvestigationConvocation.EndDate.ToLocalDateFormat(),
                    x.InvestigationConvocation.MinScore,
                    State = GeneralConstants.INVESTIGATIONCONVOCATION.STATES.VALUES.ContainsKey(x.InvestigationConvocation.State) ?
                                           GeneralConstants.INVESTIGATIONCONVOCATION.STATES.VALUES[x.InvestigationConvocation.State] : "",
                    x.InvestigationConvocation.AllowInquiries,
                    InquiryStartDate = x.InvestigationConvocation.InquiryStartDate == null ? "" : x.InvestigationConvocation.InquiryStartDate.ToLocalDateFormat(),
                    InquiryEndDate = x.InvestigationConvocation.InquiryEndDate == null ? "" : x.InvestigationConvocation.InquiryEndDate.ToLocalDateFormat(),
                    x.InvestigationConvocation.Id,
                    postulantId=x.Id
                }).FirstOrDefaultAsync();

            InvestigationConvocationName = investigationConvocation.Name;
            InvestigationConvocationCode = investigationConvocation.Code;
            InvestigationConvocationStartDate = investigationConvocation.StartDate;
            InvestigationConvocationEndDate = investigationConvocation.EndDate;
            InvestigationConvocationMinScore = investigationConvocation.MinScore;
            InvestigationConvocationState = investigationConvocation.State;
            InvestigationConvocationAllowInquiries = investigationConvocation.AllowInquiries;
            InvestigationConvocationInquiryStartDate = investigationConvocation.InquiryStartDate;
            InvestigationConvocationInquiryEndDate = investigationConvocation.InquiryEndDate;

            InvestigationConvocationId = investigationConvocation.Id;
            InvestigationConvocationPostulantId = investigationConvocation.postulantId;
            return Page();
        }

        public async Task<IActionResult> OnGetDatatableAsync(Guid investigationConvocationPostulantId, string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var convocationId = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => x.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            Expression<Func<InvestigationConvocationFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Number); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
            }

            var query = _context.InvestigationConvocationFiles
                .Where(x=>x.InvestigationConvocationId == convocationId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                   Number= x.Number,
                   Name = x.Name,
                   x.FilePath,


                }).ToListAsync();

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

        public async Task<IActionResult> OnGetInquiriesDatatableAsync(Guid investigationConvocationPostulantId,string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationConvocationInquiry, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Inquiry); break;
                case "1":
                    orderByPredicate = ((x) => x.InvestigationConvocationPostulant.User.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.InvestigationConvocationPostulant.User.FullName); break;
            }

            var query = _context.InvestigationConvocationInquiries
                .Where(x=>x.InvestigationConvocationPostulant.Id == investigationConvocationPostulantId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Inquiry.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    Inquiry=x.Inquiry,
                    UserName= x.InvestigationConvocationPostulant.User.Name,
                    FullName= x.InvestigationConvocationPostulant.User.FullName,
                    x.FilePath,
                    Id= x.Id

                }).ToListAsync();

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

        public async Task<IActionResult> OnPostCreateFileInquiryAsync(InvestigationConvocationTeacherDetailViewModel viewModel, Guid investigationConvocationPostulantId)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            //if (viewModel.File == null)
            //    return new BadRequestObjectResult("Debe subir un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            var investigationConvocationinquiry = new InvestigationConvocationInquiry
            {
                Inquiry = viewModel.Inquiry,
                InvestigationConvocationPostulantId = investigationConvocationPostulantId,
            };

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                investigationConvocationinquiry.FilePath = fileUrl;
            }



            await _investigationConvocationInquiryRepository.InsertAsync(investigationConvocationinquiry);


            return new OkObjectResult("Se enviaron las consultas a las Bases no integradas correctamente");
        }
        public async Task<IActionResult> OnPostEditFileInquiryAsync(InvestigationConvocationInquiryEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigationConvocationFileInquiry = await _investigationConvocationInquiryRepository.GetByIdAsync(viewModel.InvestigationConvocationFileInquiryId);

            if (investigationConvocationFileInquiry == null)
                return new BadRequestObjectResult("Sucedio un error");


            investigationConvocationFileInquiry.Inquiry = viewModel.Inquiry;
            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                investigationConvocationFileInquiry.FilePath = fileUrl;
            }


            await _investigationConvocationInquiryRepository.UpdateAsync(investigationConvocationFileInquiry);

            return new OkObjectResult("Se editaron las consultas a las Bases no integradas correctamente");
        }

        public async Task<IActionResult> OnGetDetailFileInquiryAsync(Guid InvestigationConvocationFileInquiryId)
        {
            var investigationConvocationInquiry = await _investigationConvocationInquiryRepository.GetByIdAsync(InvestigationConvocationFileInquiryId);

            if (investigationConvocationInquiry == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigationConvocationInquiry.FilePath,
                investigationConvocationInquiry.Inquiry,
                investigationConvocationInquiry.InvestigationConvocationPostulantId,
                investigationConvocationInquiry.Id
            };
            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnGetInquiryDeleteAsync(Guid id)
        {
            var Iconvocation = await _investigationConvocationInquiryRepository.GetByIdAsync(id);
            await _investigationConvocationInquiryRepository.DeleteAsync(Iconvocation);

            return new OkResult();
        }

    }
}
