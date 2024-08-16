using System;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/tiposdeproyecto")]
    [ApiController]
    public class InvestigationProjectController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public InvestigationProjectController(
            AkdemicContext context
            )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> SearchInvestigationTypes()
        {
            var query = _context.InvestigationProjectTypes.AsNoTracking();

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
