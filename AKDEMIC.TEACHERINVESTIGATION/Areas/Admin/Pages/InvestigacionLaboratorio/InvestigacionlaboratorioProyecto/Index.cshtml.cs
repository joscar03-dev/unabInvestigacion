using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionLaboratorio.InvestigacionlaboratorioProyectoViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.DOMAIN.Entities.InvestigacionLaboratorio;


namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionLaboratorio.InvestigacionlaboratorioProyectoPage
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


        public async Task<IActionResult> OnPostCreateAsync(InvestigacionlaboratorioProyectoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var proyecto = new DOMAIN.Entities.InvestigacionLaboratorio.InvestigacionlaboratorioProyecto
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionlaboratorioProyectos.AddAsync(proyecto);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionlaboratorioProyectoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var proyecto = await _context.InvestigacionlaboratorioProyectos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (proyecto == null) return new BadRequestObjectResult("Sucedio un error");

            proyecto.codigo = viewModel.codigo;
            proyecto.nombre = viewModel.nombre;
            proyecto.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var proyecto = await _context.InvestigacionlaboratorioProyectos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (proyecto == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionlaboratorioProyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var proyecto = await _context.InvestigacionlaboratorioProyectos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (proyecto == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                proyecto.Id,
                proyecto.codigo,
                proyecto.nombre,
                proyecto.activo,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<DOMAIN.Entities.InvestigacionLaboratorio.InvestigacionlaboratorioProyecto, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }


            var query = _context.InvestigacionlaboratorioProyectos
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.codigo,
                    x.nombre,
                    x.activo,
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
