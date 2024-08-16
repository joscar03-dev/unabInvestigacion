using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.Pages.IncubatorPostulationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.EXTERNAL_EVALUATOR)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            IDataTablesService dataTablesService,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _dataTablesService = dataTablesService;
            _context = context;
            _userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetPostulantsDatatableAsync(string search = null)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<IncubatorPostulation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.IncubatorConvocation.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.ReviewState);
                    break;
            }

            var query = _context.IncubatorPostulations
                .Where(x => x.IncubatorConvocation.IncubatorConvocationEvaluators.Any(y => y.UserId == user.Id))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.User.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.User.UserName.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    convocation = x.IncubatorConvocation.Name,
                    convocationCode = x.IncubatorConvocation.Code,
                    //x.FacultyText,
                    x.User.UserName,
                    x.User.FullName,
                    createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    x.ReviewState,
                    ReviewStateText = TeacherInvestigationConstants.IncubatorPostulation.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.IncubatorPostulation.ReviewState.VALUES[x.ReviewState] : ""
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
