using AKDEMIC.INFRASTRUCTURE.Data;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/listadetalle-de-obra")]
    [ApiController]
    public class OpusListdetailController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public OpusListdetailController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetOpusListsdetails(Guid idlist)
        {
            
           /* var query = _context.OpusListsdetails.AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Name,
                    text = x.Name,
                    idlist=x.IdList
                })
                .Where(x=> x.idlist == idlist)
                .ToListAsync();*/

            return Ok("");
        }
    }
}
