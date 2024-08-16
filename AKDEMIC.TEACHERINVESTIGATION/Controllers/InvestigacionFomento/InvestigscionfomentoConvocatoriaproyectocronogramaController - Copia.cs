using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFomento
{
    [Route("api/investigacionfomentoConovocatoriaproyectocronograma")]
    [ApiController]
    public class ApplicationConvocatoriaproyectocronogramaController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationConvocatoriaproyectocronogramaController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetInvestigacionfomentoConvocatoriaproyectocronograma(String idconvocatoriaproyecto =   "08dbec43-6a81-40af-88f5-08e40a016e3b")
        {

            
            var query = _context.InvestigacionfomentoConvocatoriaproyectoscronogramas.AsNoTracking();
            if (!string.IsNullOrEmpty(idconvocatoriaproyecto))
            {
                query = query.Where(x => x.IdConvocatoriaproyecto == Guid.Parse(idconvocatoriaproyecto))
                        .OrderBy(x => x.orden);

                
            }
            else
            {
                query = query.OrderBy(x => x.orden);
                
            }
           

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.nombremes ,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
