using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.OperativePlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.OperativePlanPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<OperativePlan, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Year);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.State);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.Unit.Name);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.Unit.User.FullName);
                    break;
            }

            var query = _context.OperativePlans
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    name = x.Name,
                    year = x.Year,
                    state = x.State,
                    filePath = x.FilePath,
                    stateText = TeacherInvestigationConstants.OperativePlan.State.VALUES.ContainsKey(x.State) ?
                        TeacherInvestigationConstants.OperativePlan.State.VALUES[x.State] : "",
                    unitText = x.Unit.Name,
                    unitBoss = x.Unit.User.FullName

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

        public async Task<IActionResult> OnGetDetailOperativePlanAsync(Guid id)
        {
            var operativePlan = await _context.OperativePlans
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (operativePlan == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                operativePlan.Id,
                operativePlan.Observation
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostObservationAsync(OperativePlanViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var operativePlan = await _context.OperativePlans
                .Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (operativePlan == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (operativePlan.State == TeacherInvestigationConstants.OperativePlan.State.APPROVED)
                return new BadRequestObjectResult("El plan operativo ya fue aprobado, por lo que no puede ser observado");

            operativePlan.Observation = viewModel.Observation;
            operativePlan.State = TeacherInvestigationConstants.OperativePlan.State.OBSERVED;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostApproveAsync(Guid id)
        {
            var operativePlan = await _context.OperativePlans
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (operativePlan == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (operativePlan.State == TeacherInvestigationConstants.OperativePlan.State.APPROVED)
                return new BadRequestObjectResult("El plan operativo ya se encuentra aprobado");

            operativePlan.State = TeacherInvestigationConstants.OperativePlan.State.APPROVED;

            await _context.SaveChangesAsync();
            return new OkResult();
        }
    }
}
