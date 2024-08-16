using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/tipo-de-obra")]
    [ApiController]
    public class OpusTypeController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public OpusTypeController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetOpusTypes()
        {
            var query = _context.OpusTypes.AsNoTracking();

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
