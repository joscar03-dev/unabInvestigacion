using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class InvestigationProjectTaskController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public InvestigationProjectTaskController(
            AkdemicContext context
        )
        {
            _context = context;
        }

        [HttpGet("select/get")]
        public async Task<IActionResult> SearchInvestigationProjectTasks()
        {
            var query = _context.InvestigationProjectTasks.AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Description,
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("{investigationProjectId}/select/get")]
        public async Task<IActionResult> GetUserRoles(Guid investigationProjectId)
        {
            var query = _context.InvestigationProjectTasks
                .Where(x => x.InvestigationProjectId == investigationProjectId)
                .OrderBy(x=>x.Description)
                .AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.Description,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
