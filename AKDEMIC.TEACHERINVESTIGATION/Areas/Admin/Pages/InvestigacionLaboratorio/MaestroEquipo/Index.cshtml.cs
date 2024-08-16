using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionLaboratorio;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionLaboratorio.MaestroEquipoViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionLaboratorio.MaestroEquipoPage
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


        public async Task<IActionResult> OnPostCreateAsync(MaestroEquipoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var equipo = new InvestigacionlaboratorioEquipo
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionlaboratorioEquipos.AddAsync(equipo);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroEquipoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var equipo = await _context.InvestigacionlaboratorioEquipos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (equipo == null) return new BadRequestObjectResult("Sucedio un error");

            equipo.codigo = viewModel.codigo;
            equipo.nombre = viewModel.nombre;
            equipo.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var equipo = await _context.InvestigacionlaboratorioEquipos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (equipo == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionlaboratorioEquipos.Remove(equipo);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var equipo = await _context.InvestigacionlaboratorioEquipos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (equipo == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                equipo.Id,
                equipo.codigo,
                equipo.nombre,
               equipo.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionlaboratorioEquipo, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.InvestigacionlaboratorioEquipos
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
