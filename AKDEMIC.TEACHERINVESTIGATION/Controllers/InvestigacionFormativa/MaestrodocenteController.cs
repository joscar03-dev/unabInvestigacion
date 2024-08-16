using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFormativa
{
    [Route("api/maestrodocente")]
    [ApiController]
    public class ApplicationDocenteController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationDocenteController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroDocente()
        {

            var queryusuario = _context.MaestroUsuarios.OrderBy(x => x.FullName).AsNoTracking();
            var query = _context.MaestroDocentes
                .AsNoTracking()
                .Join(queryusuario,
                docentes1 => docentes1.IdUser,
                usuario1 => usuario1.Id,
                (docentes1, usuario1) => new { docentes1, usuario1 });

            var result = await query
                .Select(x => new
                {
                    id = x.docentes1.Id,
                    text = x.usuario1.FullName,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
