using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFomento
{
    [Route("api/investigacionfomentoindicador")]
    [ApiController]
    public class ApplicationIndicadorController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationIndicadorController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionfomentoIndicador(string idOficina = "e0557aa7-6404-11ee-b7b1-16d13ee00159")
        {
            var query = _context.InvestigacionfomentoIndicadores
                .OrderBy(x => x.nombre)
                .Where(x => x.IdOficina == Guid.Parse(idOficina))
                .AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.nombre,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
