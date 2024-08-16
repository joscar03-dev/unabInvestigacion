using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFomento
{
    [Route("api/investigacionfomentounidadmedida")]
    [ApiController]
    public class ApplicationUnidadmedidaController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationUnidadmedidaController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionfomentoUnidadmedidaController(string idOficina= "e0557aa7-6404-11ee-b7b1-16d13ee00159")
        {
            var query = _context.InvestigacionfomentoUnidadmedidas
                .OrderBy(x => x.nombre)
               // .Where(x =>x.IdOficina== Guid.Parse(idOficina))
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
