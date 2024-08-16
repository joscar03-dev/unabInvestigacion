using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFormativa
{
    [Route("api/maestrodocentedocente")]
    [ApiController]
    public class ApplicationDocentedocenteController : ControllerBase{



        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly AkdemicContext _context;
        public ApplicationDocentedocenteController(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager

        )
        {
            _context = context;
            _userManager = userManager;

        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroDocente()
        {
            var user = await _userManager.GetUserAsync(User);

            var queryusuario = _context.MaestroUsuarios.OrderBy(x => x.FullName).AsNoTracking();
            var query = _context.MaestroDocentes
                .AsNoTracking()
                .Join(queryusuario,
                docentes1 => docentes1.IdUser,
                usuario1 => usuario1.Id,
            (docentes1, usuario1) => new { docentes1, usuario1 })
                .Where(x=> x.usuario1.Id== Guid.Parse(user.Id));

            var result = await query
                .Select(x => new
                {
                    id = x.docentes1.Id,
                    text = x.usuario1.FullName,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
