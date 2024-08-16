using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFomento
{
    [Route("api/investigacionfomentoareausuario")]
    [ApiController]
    public class ApplicationinvestigacionfomentoareausuarioController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        protected readonly AkdemicContext _context;
        public ApplicationinvestigacionfomentoareausuarioController(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager

        )
        {
            _context = context;
            _userManager = userManager;

        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionfomentoAreausuario(string idOficina = "e0557aa7-6404-11ee-b7b1-16d13ee00159")
        {
            var user = await _userManager.GetUserAsync(User);
            var queryusuario = _context.MaestroAreasusuarios
                .OrderBy(x => x.IdArea)
                .Where(x=> x.IdUser == Guid.Parse(user.Id))
                .AsNoTracking();

            var query = _context.MaestroAreas
                .AsNoTracking()
                .Join(queryusuario,
                areas1 => areas1.Id,
                usuario1 => usuario1.IdArea,
                (areas1, usuario1) => new { areas1, usuario1 });

            var result = await query
                .Select(x => new
                {
                    id = x.areas1.Id,
                    text =x.areas1.nombre,
                    idoficina=x.areas1.IdOficina,
                })
                .Where(x=>x.idoficina == Guid.Parse(idOficina))
                .ToListAsync();




            return Ok(result);
        }
    }
}
