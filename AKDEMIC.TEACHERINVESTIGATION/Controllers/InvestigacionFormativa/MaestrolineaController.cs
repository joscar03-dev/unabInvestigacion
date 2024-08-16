using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFormativa
{
    [Route("api/maestrolinea")]
    [ApiController]
    public class ApplicationLineaController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationLineaController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroLinea(String idAreaacademica = null)
        {

            var query = _context.MaestroLineas.AsNoTracking();
            if (!string.IsNullOrEmpty(idAreaacademica))
            {
                query = query.Where(x => x.IdAreaacademica == Guid.Parse(idAreaacademica))
                        .OrderBy(x => x.titulo);

                
            }
            else
            {
                query = query.OrderBy(x => x.titulo);
                
            }
           

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.titulo,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
