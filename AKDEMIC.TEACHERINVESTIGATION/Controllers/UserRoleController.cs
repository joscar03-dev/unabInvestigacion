using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/usuario-rol")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public UserRoleController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("{userId}/select/get")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var query = _context.UserRoles
                .Where(x=>x.UserId == userId)
                .OrderBy(r => r.Role.Name)
                .AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.RoleId,
                    text = x.Role.Name,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
