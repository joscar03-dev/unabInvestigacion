using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/funciones")]
    [ApiController]
    public class PublicationFunctionController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public PublicationFunctionController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetPublicationFunctions()
        {
            var query = _context.PublicationFunctions.AsNoTracking();

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
