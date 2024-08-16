using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Constants.Systems;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Student.Pages.IncubatorPostulationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.STUDENTS)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly AkdemicContext _context;

        public IndexModel(
            IDataTablesService dataTablesService,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager
        )
        {
            _dataTablesService = dataTablesService;
            _userManager = userManager;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var user = await _userManager.GetUserAsync(User);
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorPostulation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.IncubatorConvocation.Code);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.IncubatorConvocation.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Title);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.ReviewState);
                    break;

            }

            var query = _context.IncubatorPostulations
                .Where(x => x.UserId == user.Id)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.IncubatorConvocation.Code.ToUpper().Contains(searchValue.ToUpper())
                                      || x.IncubatorConvocation.Name.ToUpper().Contains(searchValue.ToUpper())
                                      || x.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    incubatorConvocationCode = x.IncubatorConvocation.Code,
                    incubatorConvocationName = x.IncubatorConvocation.Name,
                    incubatorPostulationTitle = x.Title,
                    reviewState = x.ReviewState,
                    reviewStateText = TeacherInvestigationConstants.IncubatorPostulation.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
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
