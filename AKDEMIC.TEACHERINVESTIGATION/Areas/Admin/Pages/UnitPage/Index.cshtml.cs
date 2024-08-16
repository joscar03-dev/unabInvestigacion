using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.UnitViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.UnitPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
        AkdemicContext context,
        IDataTablesService dataTablesService,
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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {


            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<Unit, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.Units
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
                    fullName = $"{x.User.UserName} - {x.User.FullName}",
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

        public async Task<IActionResult> OnGetDetailUnitAsync(Guid id)
        {
            var unit = await _context.Units
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (unit == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                unit.UserId,
                unit.Id
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditUnitAsync(UnitEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            //Hay que validar que las Unidades sean las que estan en constantes
            var unit = await _context.Units
                .Where(x => x.Id == viewModel.Id)
                .FirstOrDefaultAsync();

            if (unit == null)
                return new BadRequestObjectResult("Sucedio un error");

            //Obtendremos el rol al que se le asignará al usuario 
            var role = TeacherInvestigationConstants.Unit.BOSSROL.ContainsKey(unit.Name) ?
                TeacherInvestigationConstants.Unit.BOSSROL[unit.Name] : "";

            if (string.IsNullOrEmpty(role))
                return BadRequest("No se encontró el rol de la Unidad");

            //Validar que el rol exista en BD
            var roleExist = await _context.Roles.AnyAsync(x => x.Name == role);

            if (!roleExist)
                return BadRequest("No se encontró el rol de la Unidad");

            var newUnitBossUser = await _context.Users.Where(x => x.Id == viewModel.UserId).FirstOrDefaultAsync();
            var oldUnitBossUser = await _context.Users.Where(x => x.Id == unit.UserId).FirstOrDefaultAsync();

            //Quitarle el rol, si tiene rol
            if (oldUnitBossUser != null && await _userManager.IsInRoleAsync(unit.User, role))
            {
                var result = await _userManager.RemoveFromRoleAsync(unit.User, role);
                if (!result.Succeeded)
                    return new BadRequestObjectResult("No se ha podido eliminar el rol al usuario");
            }

            unit.UserId = viewModel.UserId;

            if (newUnitBossUser != null)
            {
                var isInRole = await _userManager.IsInRoleAsync(newUnitBossUser, role);

                if (!isInRole)
                {
                    var result = await _userManager.AddToRoleAsync(newUnitBossUser, role);
                    if (!result.Succeeded)
                        return new BadRequestObjectResult("No se ha podido asignar el rol al usuario");
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
