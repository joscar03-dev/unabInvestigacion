using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionAsesoria
{
    [Route("api/investigacionasesoriatipotrabajoinvestigacion")]
    [ApiController]
    public class ApplicationTipotrabajoinvestigacionController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationTipotrabajoinvestigacionController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionasesoriaTipotrabajoinvestigacion()
        {
            var query = _context.InvestigacionasesoriaTipotrabajoinvestigaciones
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
