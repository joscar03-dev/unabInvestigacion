using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFomento
{
    [Route("api/investigacionfomentoConovocatoriaproyectodocente")]
    [ApiController]
    public class ApplicationConvocatoriaproyectodocenteController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationConvocatoriaproyectodocenteController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionfomentoConvocatoriaproyectodocente(String iddocente=null,String idconvocatoria = null)
        {

            var query = _context.InvestigacionfomentoConvocatoriaproyectos.AsNoTracking();
            if (!string.IsNullOrEmpty(iddocente))
            {
                query = query.Where(x => x.IdDocente == Guid.Parse(iddocente) && x.IdConvocatoria == Guid.Parse(idconvocatoria))
                        .OrderBy(x => x.nombre);

                
            }
            else
            {
                query = query.OrderBy(x => x.nombre);
                
            }
           

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.nombre ,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
