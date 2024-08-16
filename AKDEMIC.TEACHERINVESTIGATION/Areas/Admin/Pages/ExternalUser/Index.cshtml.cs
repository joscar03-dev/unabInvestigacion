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
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.ExternalUser
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;

        public IndexModel(
            IDataTablesService dataTablesService,
            AkdemicContext context
            )
        {
            _dataTablesService = dataTablesService;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetExternalUserDatatableAsync(string search)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.FullName);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Dni);
                    break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt);
                    break;
            }

            var query = _context.Users
                .Where(x=>x.UserRoles.Any(y=>y.Role.Name == GeneralConstants.ROLES.EXTERNAL_EVALUATOR))
                .AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(x => x.FullName.ToLower().Trim().Contains(search.ToLower().Trim()) || x.UserName.ToLower().Trim().Contains(search.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.FullName,
                    x.Dni,
                    x.PhoneNumber
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

        public async Task<IActionResult> OnPostDeleteExternalUserAsync(string id)
        {
            var ifEvaluator = await _context.InvestigationConvocationEvaluators.Where(x => x.UserId == id).AnyAsync();
            if (ifEvaluator) 
            {
                return new BadRequestObjectResult("El usuario se encuentra asignado como evaluador.");
            }

            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            _context.Users.Remove(user);

            await _context.SaveChangesAsync();
            return new OkResult();

        }
    }
}
