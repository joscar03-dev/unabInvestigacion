using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.CoordinatorMonitor.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.CoordinatorMonitor.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_COORDINATORMONITOR)]
    public class MonitorConvocationModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAsyncRepository<MonitorConvocation> _monitorRepository;

        public MonitorConvocationModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IAsyncRepository<MonitorConvocation> monitorRepository,
            IDataTablesService dataTablesService
            )
        {
            _monitorRepository = monitorRepository;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public Guid InvestigationConvocationId { get; set; }

        public void OnGet(Guid investigationConvocationId)
        {
            InvestigationConvocationId = investigationConvocationId;
        }

        public async Task<IActionResult> OnGetMonitorDatatableAsync(Guid investigationConvocationId, string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<MonitorConvocation, dynamic>> orderByPredicate = null;
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

            var query = _context.MonitorConvocations.Where(x => x.InvestigationConvocationId == investigationConvocationId).AsNoTracking();

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
                    InvestigationConvocationId = x.InvestigationConvocationId,
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
        public async Task<IActionResult> OnGetMonitorDeleteAsync(Guid investigationConvocationId, string userId)
        {
            object[] keyValues = new object[] { userId, investigationConvocationId };
            var superVisor = await _monitorRepository.GetByIdAsync(keyValues);
            if (superVisor == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var user = await _context.Users.Where(x => x.Id == superVisor.UserId).FirstOrDefaultAsync();

            var isInRole = await _userManager.IsInRoleAsync(user, GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_MONITOR);

            if (isInRole)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_MONITOR);
                if (!result.Succeeded)
                    return new BadRequestObjectResult("No se le ha podido quitar el rol");
            }

            await _monitorRepository.DeleteAsync(superVisor);
            return new OkResult();
        }
        public async Task<IActionResult> OnPostAsync(MonitorCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revisa el formulario");

            var user = await _context.Users.Where(x => x.Id == model.UserId).FirstOrDefaultAsync();
            var convocationExist = await _context.InvestigationConvocations.AnyAsync(x => x.Id == model.InvestigationConvocationId);

            if (user == null || !convocationExist)
                return new BadRequestObjectResult("Sucedio un error");


            //que el usuario no este asignado a la convocatoria
            var convocationSupervisorExist = await _context.MonitorConvocations.AnyAsync(x => x.UserId == model.UserId && x.InvestigationConvocationId == model.InvestigationConvocationId);

            if (convocationSupervisorExist)
                return new BadRequestObjectResult("El usuario ya esta asignado en esta convocatoria");

            var monitor = new MonitorConvocation
            {
                UserId = model.UserId,
                InvestigationConvocationId = model.InvestigationConvocationId,
            };

            var isInRole = await _userManager.IsInRoleAsync(user, GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_MONITOR);

            if (!isInRole)
            {
                var result = await _userManager.AddToRoleAsync(user, GeneralConstants.ROLES.INVESTIGATIONCONVOCATION_MONITOR);
                if (!result.Succeeded)
                    return new BadRequestObjectResult("No se ha podido asignar el rol al usuario");
            }

            await _monitorRepository.InsertAsync(monitor);
            return new OkResult();
        }
    }
}
