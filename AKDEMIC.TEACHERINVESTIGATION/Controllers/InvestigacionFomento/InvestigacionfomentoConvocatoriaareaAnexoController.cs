using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFomento
{
    [Route("api/investigacionfomentoconvocatoriaareaanexo")]
    [ApiController]
    public class ApplicationConvocatoriaareaanexoController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationConvocatoriaareaanexoController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionfomentoConvocatoriaarea(String idconvocatoria=null)
        {

            Console.WriteLine(idconvocatoria);
            var queryconvocatoria = _context.InvestigacionfomentoConvocatorias.AsNoTracking();
            var queryflujoarea = _context.InvestigacionfomentoFlujosareas
                .Where(x=>x.retornadocente=="1").AsNoTracking();

            var query = _context.MaestroAreas.AsNoTracking();

            var result = await query
                .Join(queryflujoarea,
                    area1 => area1.Id,
                    flujoarea1 => flujoarea1.IdArea,
                    (area1, flujoarea1) => new { area1, flujoarea1 })
                .Join(queryconvocatoria,
                  area2 => area2.flujoarea1.IdFlujo,
                  convocatoria1 => convocatoria1.IdFlujo,
                  (area2, convocatoria1) => new { area2, convocatoria1 })
                .Select(x => new
                {
                    id = x.area2.area1.Id,
                    text = x.area2.area1.nombre,
                    IdConvocatoria = x.convocatoria1.Id,
                })
                .Where(x => x.IdConvocatoria == Guid.Parse(idconvocatoria))
                .ToListAsync();

            return Ok(result);
        }
    }
}
