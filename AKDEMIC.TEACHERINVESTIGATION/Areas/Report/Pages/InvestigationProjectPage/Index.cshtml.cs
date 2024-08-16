using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using AKDEMIC.CORE.Services;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestigationProjectPage
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
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(Guid? investigationProjectTypeId = null, Guid? financingInvestigationId = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationProject, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = (x) => x.InvestigationConvocationPostulant.ProjectTitle;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InvestigationConvocationPostulant.User.FullName;
                    break;
            }

            var query = _context.InvestigationProjects
                .AsNoTracking();

            if (investigationProjectTypeId != null)
                query = query.Where(x => x.InvestigationProjectTypeId == investigationProjectTypeId);

            if (financingInvestigationId != null)
                query = query.Where(x => x.InvestigationConvocationPostulant.FinancingInvestigationId == financingInvestigationId);


            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.Id,
                    ProjectTitle = x.InvestigationConvocationPostulant.ProjectTitle,
                    UserFullName = x.InvestigationConvocationPostulant.User.FullName
                    //Id = x.Id,
                    //Code = x.Code,
                    //Name = x.Name,
                    //totalPostulations = x.InvestigationConvocationPostulants
                    //    .Where(y => y.InvestigationConvocationId == x.Id)
                    //    .Count(),
                    //totalProjectApproveds = x.InvestigationConvocationPostulants
                    //    .Where(y => y.InvestigationConvocationId == x.Id && y.ProjectState == TeacherInvestigationConstants.ConvocationPostulant.ProjectState.ACCEPTED)
                    //    .Count()
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
