using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationProjectPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class ProjectReportModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public ProjectReportModel(
            AkdemicContext context,
            IDataTablesService dataTablesService
        )
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        public Guid InvestigationProjectId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid investigationProjectId)
        {
            var investigationProject = await _context.InvestigationProjects.Where(x => x.Id == investigationProjectId).FirstOrDefaultAsync();

            if (investigationProject == null)
                return RedirectToPage("Index");

            InvestigationProjectId = investigationProject.Id;

            return Page();
        }


        public async Task<IActionResult> OnPostCreateAsync(InvestigationProjectReportViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            if (string.IsNullOrEmpty(viewModel.ExpirationDate))
            {

                return new BadRequestObjectResult("Ingresar una fecha de expiración");
            }

            var investigationProjectReport = new InvestigationProjectReport
            {
                ExpirationDate = ConvertHelpers.DatepickerToUtcDateTime(viewModel.ExpirationDate),
                InvestigationProjectId=viewModel.InvestigationProjectId,
                Name = viewModel.Name
            };

            await _context.InvestigationProjectReports.AddAsync(investigationProjectReport);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigationProjectReportEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigationProjectReport = await _context.InvestigationProjectReports.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigationProjectReport == null) return new BadRequestObjectResult("Sucedio un error");

            if (string.IsNullOrEmpty(viewModel.ExpirationDate))
            {

                return new BadRequestObjectResult("Ingresar una fecha de expiración");
            }

            investigationProjectReport.Name = viewModel.Name;
            investigationProjectReport.ExpirationDate = ConvertHelpers.DatepickerToUtcDateTime(viewModel.ExpirationDate);
            investigationProjectReport.InvestigationProjectId = viewModel.InvestigationProjectId;


            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigationProjectReport = await _context.InvestigationProjectReports.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigationProjectReport == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigationProjectReports.Remove(investigationProjectReport);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigationProjectReport = await _context.InvestigationProjectReports.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigationProjectReport == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigationProjectReport.Id,
                investigationProjectReport.Name,
                ExpirationDate = investigationProjectReport.ExpirationDate.ToLocalDateFormat(),
                investigationProjectReport.InvestigationProjectId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigationProjectReport, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.ExpirationDate;
                    break;
            }

            var query = _context.InvestigationProjectReports
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    ExpirationDate= x.ExpirationDate.ToLocalDateFormat(),
                    x.Name,
                    x.ReportUrl,
                    x.InvestigationProjectId
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
