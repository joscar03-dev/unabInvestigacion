using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.CoordinatorMonitor.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_COORDINATORMONITOR)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService
        )
        {
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var user = await _userManager.GetUserAsync(User);
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
                    orderByPredicate = (x) => x.InscriptionStartDate;
                    break;
                case "3":
                    orderByPredicate = (x) => x.InscriptionEndDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.StartDate;
                    break;
                case "5":
                    orderByPredicate = (x) => x.EndDate;
                    break;
                case "6":
                    orderByPredicate = (x) => x.MinScore;
                    break;
            }

            var query = _context.InvestigationConvocations
                .Where(x => x.CoordinatorMonitorConvocations.Any(y => y.UserId == user.Id))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Code.ToUpper().Contains(searchValue.ToUpper())
                                      || x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    inscriptionStartDate = x.InscriptionStartDate.ToLocalDateFormat(),
                    inscriptionEndDate = x.InscriptionEndDate.ToLocalDateFormat(),
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat(),
                    minScore = x.MinScore
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
