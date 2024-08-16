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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Monitor.Pages.InvestigationConvocationPostulantPage
{
    [Authorize(Roles = GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_MONITOR)]
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

            Expression<Func<InvestigationConvocationPostulant, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = (x) => x.InvestigationConvocation.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InvestigationConvocation.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ProjectTitle;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
            }

            var query = _context.InvestigationConvocationPostulants
                .Where(x => x.InvestigationConvocation.MonitorConvocations.Any(y => y.UserId == user.Id))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.InvestigationConvocation.Code.ToUpper().Contains(searchValue.ToUpper())
                                     || x.InvestigationConvocation.Name.ToUpper().Contains(searchValue.ToUpper())
                                     || x.ProjectTitle.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.Id,
                    Code = x.InvestigationConvocation.Code,
                    Name = x.InvestigationConvocation.Name,
                    ProjectTitle = x.ProjectTitle,
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
