using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.TeamMemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.TeamMemberRolePage
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
        public async Task<IActionResult> OnPostCreateAsync(TeamMemberCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var teamMemberRole = new TeamMemberRole
            {
                Name = viewModel.Name
            };

            await _context.TeamMemberRoles.AddAsync(teamMemberRole);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(TeamMemberEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var teamMemberRole = await _context.TeamMemberRoles.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (teamMemberRole == null) return new BadRequestObjectResult("Sucedio un error");

            teamMemberRole.Name = viewModel.Name;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var teamMemberRole = await _context.TeamMemberRoles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (teamMemberRole == null) return new BadRequestObjectResult("Sucedio un error");

            _context.TeamMemberRoles.Remove(teamMemberRole);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var teamMemberRole = await _context.TeamMemberRoles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (teamMemberRole == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                teamMemberRole.Id,
                teamMemberRole.Name
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<TeamMemberRole, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.TeamMemberRoles
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x =>x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name
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
