using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionLaboratorio;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionLaboratorio.InvestigacionlaboratorioHorarioViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionLaboratorio.InvestigacionlaboratorioHorarioPage
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

        public InvestigacionlaboratorioHorarioCreateViewModel fechaini { get; set; }
        public InvestigacionlaboratorioHorarioCreateViewModel fechafin { get; set; }
        public async Task<IActionResult> OnPostCreateAsync(InvestigacionlaboratorioHorarioCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var horario = new DOMAIN.Entities.InvestigacionLaboratorio.InvestigacionlaboratorioHorario
            {
                IdLaboratorio=viewModel.IdLaboratorio,
                IdDocente=viewModel.IdDocente,
                IdEquipo=viewModel.IdEquipo,
                IdProyecto=viewModel.IdProyecto,
                fechaini=viewModel.fechaini,
                fechafin=viewModel.fechafin,
                codigo = viewModel.codigo,
                actividad = viewModel.actividad,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionlaboratorioHorarios.AddAsync(horario);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionlaboratorioHorarioEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var horario = await _context.InvestigacionlaboratorioHorarios.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (horario == null) return new BadRequestObjectResult("Sucedio un error");

            horario.codigo = viewModel.codigo;
            horario.actividad = viewModel.actividad;
            horario.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var horario = await _context.InvestigacionlaboratorioHorarios.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (horario == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionlaboratorioHorarios.Remove(horario);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var horario = await _context.InvestigacionlaboratorioHorarios.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (horario == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                horario.Id,
                horario.codigo,
                horario.actividad,
                horario.activo ,
                horario.IdLaboratorio,
                horario.IdDocente,
                horario.IdEquipo,
                horario.IdProyecto,
                fechaini= horario.fechaini.ToLocalDateTimeFormat(),
                fechafin= horario.fechafin.ToLocalDateTimeFormat(),
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<DOMAIN.Entities.InvestigacionLaboratorio.InvestigacionlaboratorioHorario, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.actividad;
                    break;
            }


            var querylaboratorio = _context.InvestigacionlaboratorioLaboratorios.AsNoTracking();
            var querydocente = _context.MaestroDocentes.AsNoTracking();
            var queryusuario = _context.MaestroUsuarios.AsNoTracking();
            var querproyecto = _context.InvestigacionlaboratorioProyectos.AsNoTracking();

            var query = _context.InvestigacionlaboratorioHorarios.AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.actividad.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querylaboratorio,
                  queryhorario1=>queryhorario1.IdLaboratorio,
                  querylaboratorio1=>querylaboratorio1.Id,
                  (queryhorario1, querylaboratorio1) => new { queryhorario1 , querylaboratorio1 })
                .Join(querproyecto,
                  queryhorario2 => queryhorario2.queryhorario1.IdProyecto,
                  queryproyecto1 => queryproyecto1.Id,
                  (queryhorario2, queryproyecto1) => new { queryhorario2, queryproyecto1 })
                .Join(querydocente,
                  queryhorario3 => queryhorario3.queryhorario2.queryhorario1.IdDocente,
                  querydocente1 => querydocente1.Id,
                  (queryhorario3, querydocente1) => new { queryhorario3, querydocente1 })
                .Join(queryusuario,
                  querydocente2 => querydocente2.querydocente1.IdUser,
                  queryusuario1 => queryusuario1.Id,
                  (querydocente2, queryusuario1) => new { querydocente2, queryusuario1 })
                .Select(x => new
                {
                    Id = x.querydocente2.queryhorario3.queryhorario2.queryhorario1.Id,
                    codigo = x.querydocente2.queryhorario3.queryhorario2.queryhorario1.codigo,
                    actividad = x.querydocente2.queryhorario3.queryhorario2.queryhorario1.actividad,
                    fechaini = x.querydocente2.queryhorario3.queryhorario2.queryhorario1.fechaini.ToLocalDateTimeFormat(),
                    fechafin = x.querydocente2.queryhorario3.queryhorario2.queryhorario1.fechafin.ToLocalDateTimeFormat(),
                    activo = x.querydocente2.queryhorario3.queryhorario2.queryhorario1.activo,
                    nombredocente = x.queryusuario1.FullName,
                    nombreproyecto = x.querydocente2.queryhorario3.queryproyecto1.nombre,
                    nombrelaboratorio = x.querydocente2.queryhorario3.queryhorario2.querylaboratorio1.nombre,

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
