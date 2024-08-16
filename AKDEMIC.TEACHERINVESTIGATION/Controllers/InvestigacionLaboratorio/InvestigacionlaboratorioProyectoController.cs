using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionLaboratorio
{
    [Route("api/investigacionlaboratorioproyecto")]
    [ApiController]
    public class ApplicationProyectocontroller : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationProyectocontroller(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvetigacionlaboratorioProyecto()
        {

            var query = _context.InvestigacionlaboratorioProyectos
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
