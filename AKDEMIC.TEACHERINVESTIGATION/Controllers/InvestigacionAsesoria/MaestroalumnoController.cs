using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionAsesoria
{
    [Route("api/maestroalumno")]
    [ApiController]
    public class ApplicationAlumnoController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationAlumnoController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroAlumno()
        {

            var queryusuario = _context.MaestroUsuarios.OrderBy(x => x.FullName).AsNoTracking();
            var query = _context.MaestroAlumnos
                .AsNoTracking()
                .Join(queryusuario,
                alumnos1 => alumnos1.IdUser,
                usuario1 => usuario1.Id,
                (alumnos1, usuario1) => new { alumnos1, usuario1 });

            var result = await query
                .Select(x => new
                {
                    id = x.alumnos1.Id,
                    text = x.usuario1.FullName,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
