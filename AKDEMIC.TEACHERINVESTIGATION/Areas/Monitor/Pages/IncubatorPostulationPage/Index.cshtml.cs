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
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Monitor.Pages.IncubatorPostulationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.INCUBATORCONVOCATION_MONITOR)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;

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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var user = await _userManager.GetUserAsync(User);

            Expression<Func<IncubatorPostulation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = (x) => x.IncubatorConvocation.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IncubatorConvocation.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Title;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
            }

            var query = _context.IncubatorPostulations
                .Where(x => x.IncubatorConvocation.IncubatorMonitors.Any(y => y.UserId == user.Id))
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
                    Id = x.Id,
                    Code = x.IncubatorConvocation.Code,
                    Name = x.IncubatorConvocation.Name,
                    Title = x.Title,
                    FullName = x.User.FullName,
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
