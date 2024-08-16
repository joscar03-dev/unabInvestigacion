using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public RoleController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetRoles()
        {
            var query = _context.Roles
                .Where(x => !(x.Name == GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT ||
                        x.Name == GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT ||
                        x.Name == GeneralConstants.ROLES.PUBLICATION_UNIT ||
                        x.Name == GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT))
                .OrderBy(x => x.Name)
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
