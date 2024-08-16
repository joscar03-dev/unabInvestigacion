using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/lineasdeinvestigacion")]
    [ApiController]
    public class ResearchLineController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ResearchLineController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("linea/{researchLineCategoryId}/select/get")]
        public async Task<IActionResult> SearchResearchLine(Guid researchLineCategoryId)
        {
            var query = _context.ResearchLines
                .Where(x=> x.ResearchLineCategoryId == researchLineCategoryId)
                .AsNoTracking();

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
