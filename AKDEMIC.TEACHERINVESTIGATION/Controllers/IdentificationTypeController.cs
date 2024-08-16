using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/tipo-de-identificacion")]
    [ApiController]
    public class IdentificationTypeController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public IdentificationTypeController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetIndexes()
        {
            var query = _context.IdentificationTypes.AsNoTracking();

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
