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
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Constants.Systems;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.IncubatorPostulantPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN )]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService
        )
        {
            _dataTablesService = dataTablesService;
            _context = context;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
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
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.ReviewState);
                    break;

            }

            var query = _context.IncubatorPostulations
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                string searchTrimed = searchValue.Trim();
                query = query.Where(x => x.User.UserName.ToUpper().Contains(searchTrimed.ToUpper()) ||
                                        x.User.PaternalSurname.ToUpper().Contains(searchTrimed.ToUpper()) ||
                                        x.User.MaternalSurname.ToUpper().Contains(searchTrimed.ToUpper()) ||
                                        x.User.Name.ToUpper().Contains(searchTrimed.ToUpper()) ||
                                        x.User.FullName.ToUpper().Contains(searchTrimed.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorConvocation.Code,
                    x.IncubatorConvocation.Name,
                    x.Title,
                    x.User.UserName,
                    x.User.FullName,
                    ReviewState = TeacherInvestigationConstants.IncubatorPostulation.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
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
