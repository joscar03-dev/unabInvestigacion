using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestigacionFormativa.InvestigacionformativaInfomefinalPage
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

        public async Task<IActionResult> OnGetDatatableAsync(string startDate, string endDate)
        {

            int flag = 1;
            // var data2 = _context.spPlantrabajos.FromSqlInterpolated($"call sp_investigacionformativa_plantrabajos(@fla={flag})").ToList();

            var querytipoevento = _context.InvestigacionformativaTipoeventos.AsNoTracking();
            var querytiporesultado = _context.InvestigacionformativaTiporesultados.AsNoTracking();
            var queryconfiguracionanio = _context.InvestigacionformativaConfiguracionanios.AsNoTracking();
            var queryactividades = _context.InvestigacionformativaPlantrabajosactividades.AsNoTracking();
            var querycarrera = _context.MaestroCarreras.AsNoTracking();
            var queryusuario = _context.MaestroUsuarios.AsNoTracking();
            var querydocente = _context.MaestroDocentes.AsNoTracking();
            
            var query = _context.InvestigacionformativaPlantrabajos
                .Join(queryactividades,
                 queryplantrabajo1=>queryplantrabajo1.Id,
                 queryplantrabajoactividad1=>queryplantrabajoactividad1.IdPlantrabajo,
                 (queryplantrabajo1, queryplantrabajoactividad1) => new { queryplantrabajo1, queryplantrabajoactividad1  })
                .Join(querycarrera,
                queryplantrabajo2=>queryplantrabajo2.queryplantrabajo1.IdCarrera,
                querycarrera1=>querycarrera1.Id,
                (queryplantrabajo2, querycarrera1) => new { queryplantrabajo2, querycarrera1 })
                 .Join(querydocente,
                queryplantrabajo3 => queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.IdDocente,
                querydocente1 => querydocente1.Id,
                (queryplantrabajo3, querydocente1) => new { queryplantrabajo3, querydocente1 })
                 .Join(queryusuario,
                queryplantrabajo4=> queryplantrabajo4.querydocente1.IdUser,
                queryusuario1 => queryusuario1.Id,
                (queryplantrabajo4, queryusuario1) => new { queryplantrabajo4, queryusuario1 })
                  .Join(queryconfiguracionanio,
                queryplantrabajo5 => queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.IdAnio,
                queryanio1 => queryanio1.Id,
                (queryplantrabajo5, queryanio1) => new { queryplantrabajo5, queryanio1 })
                   .Join(querytipoevento,
                queryplantrabajo6 => queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.IdTipoevento,
                queryevento1 => queryevento1.Id,
                (queryplantrabajo6, queryevento1) => new { queryplantrabajo6, queryevento1 })
                    .Join(querytiporesultado,
                queryplantrabajo7 => queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.IdTiporesultado,
                queryresultado1 => queryresultado1.Id,
                (queryplantrabajo7, queryresultado1) => new { queryplantrabajo7, queryresultado1 })
                .Select(x => new
                {
                    
                    x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.Id,
                    x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.IdDocente,
                    x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.IdCarrera,
                    x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajoactividad1.informefinal,
                    archivourl= x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajoactividad1.anexourl??"",
                    nombrecarrera = x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.querycarrera1.nombre,
                    fullname =  x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryusuario1.FullName,
                    x.queryplantrabajo7.queryplantrabajo6.queryplantrabajo5.queryplantrabajo4.queryplantrabajo3.queryplantrabajo2.queryplantrabajo1.titulo,
                    anio = x.queryplantrabajo7.queryplantrabajo6.queryanio1.nombre,
                    nombretipoevento=x.queryplantrabajo7.queryevento1.nombre,
                    nombretiporesultado = x.queryresultado1.nombre,


                })
                .Where(x=>x.informefinal=="1" && x.archivourl!="")
                .AsNoTracking();

            var result2 = new DataTablesStructs.ReturnedData<object>
            {
                Data = query,
                DrawCounter = 1,
                RecordsFiltered = 1,
                RecordsTotal = query.Count(),
            };

            

            return new OkObjectResult(result2);

        }

        public void OnGet()
        {
        }
    }
}
