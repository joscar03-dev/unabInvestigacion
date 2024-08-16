using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionAsesoria
{
    [Route("api/maestroanio")]
    [ApiController]
    public class ApplicationAnioController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationAnioController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroAnio()
        {

            var query = _context.MaestroAnios.OrderBy(x => x.anio).AsNoTracking();
            
            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.anio,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
