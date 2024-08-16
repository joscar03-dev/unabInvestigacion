using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/centro-investigacion")]
    [ApiController]
    public class ResearchCenterController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ResearchCenterController(
            AkdemicContext context
        )
        {
            _context = context;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetResearchCenterSelect()
        {
            var result = await _context.ResearchCenters
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Name
                })
                .ToListAsync();


            return Ok(result);
        }
    }
}
