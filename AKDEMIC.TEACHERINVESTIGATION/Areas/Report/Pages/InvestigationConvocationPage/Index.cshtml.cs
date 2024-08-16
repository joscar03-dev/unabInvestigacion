using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
    AkdemicContext context,
    IDataTablesService dataTablesService
)
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        public async Task<IActionResult> OnGetDatatableAsync(string startDate, string endDate)
        {
            

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationConvocation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.InvestigationConvocationPostulants
                    .Where(y => y.InvestigationConvocationId == x.Id)
                    .Count();
                    break;
                case "3":
                    orderByPredicate = (x) => x.InvestigationConvocationPostulants
                .Where(y => y.InvestigationConvocationId == x.Id && y.ProjectState == TeacherInvestigationConstants.ConvocationPostulant.ProjectState.ACCEPTED)
                .Count();
                    break;
            }

            var query = _context.InvestigationConvocations
                .AsNoTracking();

            if(!string.IsNullOrEmpty(startDate))
            {
                var startDateDT = ConvertHelpers.DatepickerToUtcDateTime(startDate);
                query = query.Where(x => x.StartDate.Date >= startDateDT.Date);

            }

            if (!string.IsNullOrEmpty(endDate))
            {
                var endDateDT = ConvertHelpers.DatepickerToUtcDateTime(endDate);
                query = query.Where(x => x.StartDate.Date <= endDateDT.Date);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    totalPostulations = x.InvestigationConvocationPostulants
                        .Where(y => y.InvestigationConvocationId == x.Id)
                        .Count(),
                    totalProjectApproveds = x.InvestigationConvocationPostulants
                        .Where(y => y.InvestigationConvocationId == x.Id && y.ProjectState == TeacherInvestigationConstants.ConvocationPostulant.ProjectState.ACCEPTED)
                        .Count()
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

        public void OnGet()
        {
        }
    }
}
