using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFormativa
{
    [Route("api/maestrousuario")]
    [ApiController]
    public class ApplicationUsuarioController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationUsuarioController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroUsuario()
        {
            var query = _context.MaestroUsuarios         
                .OrderBy(x => x.FullName)
                .AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.FullName,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
