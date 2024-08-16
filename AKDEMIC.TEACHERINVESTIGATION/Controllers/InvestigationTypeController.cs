using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/tiposdeinvestigacion")]
    [ApiController]
    public class InvestigationTypeController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public InvestigationTypeController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> SearchInvestigationTypes()
        {
            var query = _context.InvestigationTypes.AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
