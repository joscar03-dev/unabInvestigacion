using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/orden-autoria")]
    [ApiController]
    public class AuthorShipOrderController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public AuthorShipOrderController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetAuthorShipOrders()
        {
            var query = _context.AuthorshipOrders.AsNoTracking();

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
