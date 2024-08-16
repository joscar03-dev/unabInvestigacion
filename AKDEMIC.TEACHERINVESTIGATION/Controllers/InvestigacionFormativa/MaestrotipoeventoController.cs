using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFormativa
{
    [Route("api/investigacionformativatipoevento")]
    [ApiController]
    public class ApplicationTipoeventoController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationTipoeventoController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionformativaTipoevento()
        {
            var query = _context.InvestigacionformativaTipoeventos             
                .OrderBy(x => x.nombre)
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
