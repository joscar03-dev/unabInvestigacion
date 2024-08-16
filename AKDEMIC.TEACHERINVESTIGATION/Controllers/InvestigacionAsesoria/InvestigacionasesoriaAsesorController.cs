using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionAsesoria
{
    [Route("api/investigacionasesoriaasesor")]
    [ApiController]
    public class ApplicationAsesorController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationAsesorController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionasesoriaAsesor()
        {

            var queryusuario = _context.MaestroUsuarios.OrderBy(x => x.FullName).AsNoTracking();
            var query = _context.InvestigacionasesoriaAsesores
                .AsNoTracking()
                .Join(queryusuario,
                asesores1 => asesores1.IdUser,
                usuario1 => usuario1.Id,
                (asesores1, usuario1) => new { asesores1, usuario1 });

            var result = await query
                .Select(x => new
                {
                    id = x.asesores1.Id,
                    text = x.usuario1.FullName,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
