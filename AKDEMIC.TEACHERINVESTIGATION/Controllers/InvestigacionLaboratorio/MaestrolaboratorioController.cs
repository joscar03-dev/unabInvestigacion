using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionLaboratorio
{
    [Route("api/maestrolaboratorio")]
    [ApiController]
    public class ApplicationLaboratoriocontroller : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationLaboratoriocontroller(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroLaboratorio()
        {

            var query = _context.InvestigacionlaboratorioLaboratorios
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
