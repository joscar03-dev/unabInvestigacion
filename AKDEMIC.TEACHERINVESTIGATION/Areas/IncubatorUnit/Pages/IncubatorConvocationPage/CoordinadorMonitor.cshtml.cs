using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT)]
    public class CoordinadorMonitorModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAsyncRepository<IncubatorCoordinatorMonitor> _coordinatormonitorRepository;

        public CoordinadorMonitorModel(
    AkdemicContext context,
    UserManager<ApplicationUser> userManager,
    IAsyncRepository<IncubatorCoordinatorMonitor> coordinatormonitorRepository,
    IDataTablesService dataTablesService
    )
        {
            _coordinatormonitorRepository = coordinatormonitorRepository;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public Guid IncubatorConvocationId { get; set; }

        public void OnGet(Guid incubatorConvocationId)
        {
            IncubatorConvocationId = incubatorConvocationId;
        }

        public async Task<IActionResult> OnGetCoordinatorMonitorDatatableAsync(Guid incubatorConvocationId, string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorCoordinatorMonitor, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.User.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.User.MaternalSurname);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.PaternalSurname);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.Dni);
                    break;
            }

            var query = _context.IncubatorCoordinatorMonitors.Where(x => x.IncubatorConvocationId == incubatorConvocationId).AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.User.FullName.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    UserName = x.User.UserName,
                    Name = x.User.Name,
                    UserId = x.UserId,
                    MaternalSurname = x.User.MaternalSurname,
                    PaternalSurname = x.User.PaternalSurname,
                    IncubatorConvocationId = x.IncubatorConvocationId,
                    Dni = x.User.Dni,
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
        public async Task<IActionResult> OnGetCoordinatorMonitorDeleteAsync(Guid incubatorConvocationId, string userId)
        {
            object[] keyValues = new object[] { userId, incubatorConvocationId };
            var superVisor = await _coordinatormonitorRepository.GetByIdAsync(keyValues);
            if (superVisor == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var user = await _context.Users.Where(x => x.Id == superVisor.UserId).FirstOrDefaultAsync();

            var isInRole = await _userManager.IsInRoleAsync(user, GeneralConstants.ROLES.INCUBATORCONVOCATION_COORDINATORMONITOR);

            if (isInRole)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, GeneralConstants.ROLES.INCUBATORCONVOCATION_COORDINATORMONITOR);
                if (!result.Succeeded)
                    return new BadRequestObjectResult("No se le ha podido quitar el rol");
            }

            await _coordinatormonitorRepository.DeleteAsync(superVisor);
            return new OkResult();
        }
        public async Task<IActionResult> OnPostAsync(IncubatorConvocationMonitorCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revisa el formulario");

            var user = await _context.Users.Where(x => x.Id == model.UserId).FirstOrDefaultAsync();
            var convocationExist = await _context.IncubatorConvocations.AnyAsync(x => x.Id == model.IncubatorConvocationId);

            if (user == null || !convocationExist)
                return new BadRequestObjectResult("Sucedio un error");


            //que el usuario no este asignado a la convocatoria
            var convocationSupervisorExist = await _context.IncubatorCoordinatorMonitors.AnyAsync(x => x.UserId == model.UserId && x.IncubatorConvocationId == model.IncubatorConvocationId);

            if (convocationSupervisorExist)
                return new BadRequestObjectResult("El usuario ya esta asignado en esta convocatoria");

            var monitor = new IncubatorCoordinatorMonitor
            {
                UserId = model.UserId,
                IncubatorConvocationId = model.IncubatorConvocationId,
            };

            var isInRole = await _userManager.IsInRoleAsync(user, GeneralConstants.ROLES.INCUBATORCONVOCATION_COORDINATORMONITOR);

            if (!isInRole)
            {
                var result = await _userManager.AddToRoleAsync(user, GeneralConstants.ROLES.INCUBATORCONVOCATION_COORDINATORMONITOR);
                if (!result.Succeeded)
                    return new BadRequestObjectResult("No se ha podido asignar el rol al usuario");
            }

            await _coordinatormonitorRepository.InsertAsync(monitor);
            return new OkResult();
        }
    }
}