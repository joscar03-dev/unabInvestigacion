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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.UserPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;

        protected readonly AkdemicContext _context;

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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
            }

            //Solo usuarios creados por nuestro sistema
            var query = _context.Users
                .IgnoreQueryFilters()
                .Where(x => x.AuthenticationUserId == null)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.UserName.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.Name,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Email,
                    State = (x.DeletedAt == null && x.DeletedBy == null) ? "Activo" : "Eliminado",
                    isDeleted = (x.DeletedAt != null && x.DeletedBy != null)
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

        public async Task<IActionResult> OnPostUserDeleteAsync(string userId)
        {
            var user = await _context.Users.Where(x=>x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return BadRequest("Sucedio un Error");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostUserRestartAsync(string userId)
        {
            var user = await _context.Users
                .IgnoreQueryFilters()
                .Where(x => x.Id == userId).FirstOrDefaultAsync();
            if (user == null)
                return BadRequest("Sucedio un Error");

            user.DeletedAt = null;
            user.DeletedBy = null;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
