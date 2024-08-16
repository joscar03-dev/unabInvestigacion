using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.ViewModels.OperativePlanViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.Pages.OperativePlanPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT + "," +
        GeneralConstants.ROLES.PUBLICATION_UNIT + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _storageCredentials = storageCredentials;
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var user = await _userManager.GetUserAsync(User);

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
            }

            var query = _context.OperativePlans
               .Where(x => x.Unit.UserId == user.Id)
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
                    stateText = TeacherInvestigationConstants.OperativePlan.State.VALUES.ContainsKey(x.State) ?
                        TeacherInvestigationConstants.OperativePlan.State.VALUES[x.State] : ""
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

        public async Task<IActionResult> OnPostCreate(OperativePlanCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");


            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.OPERATIVE_PLAN,
            Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var user = await _userManager.GetUserAsync(User);

            var unit = await _context.Units
                .Where(x => x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (unit == null)
            {
                return new BadRequestObjectResult("El usuario no esta asignado a una unidad");
            }

            var hasOperativePlanThisYear = await _context.OperativePlans
                .AnyAsync(x => x.UnitId == unit.Id && x.Year == DateTime.UtcNow.Year);

            if (hasOperativePlanThisYear)
            {
                return new BadRequestObjectResult("Ya existe un registro para este año");
            }

            var operativePlan = new OperativePlan
            {

                Name = viewModel.Name,
                UnitId = unit.Id,
                State = TeacherInvestigationConstants.OperativePlan.State.PENDING,
                FilePath = fileUrl,
                Year = DateTime.UtcNow.Year,
                RegisterDate = DateTime.UtcNow
            };

            await _context.OperativePlans.AddAsync(operativePlan);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(OperativePlanEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var operativePlan = await _context.OperativePlans
                .Include(x => x.Unit)
                .Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (operativePlan == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (operativePlan.State == TeacherInvestigationConstants.OperativePlan.State.APPROVED)
                return new BadRequestObjectResult("El plan operativo ya fue aprobado, por lo que no puede ser editado");

            var user = await _userManager.GetUserAsync(User);

            if (operativePlan.Unit.UserId != user.Id)
                return new BadRequestObjectResult("Usted no se encuentra asignado a esta unidad");

            operativePlan.Name = viewModel.Name;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.OPERATIVE_PLAN,
                    Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                operativePlan.FilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
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
                operativePlan.Name,
                operativePlan.FilePath,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var operativePlan = await _context.OperativePlans
                .Include(x => x.Unit)
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (operativePlan == null) 
                return new BadRequestObjectResult("Sucedio un error");

            var user = await _userManager.GetUserAsync(User);

            if (operativePlan.Unit.UserId != user.Id)
                return new BadRequestObjectResult("Usted no se encuentra asignado a esta unidad");

            if (operativePlan.State != TeacherInvestigationConstants.OperativePlan.State.PENDING)
                return new BadRequestObjectResult("Solo se puede eliminar el plan operativo que este en estado pendiente");

            _context.OperativePlans.Remove(operativePlan);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

    }


}
