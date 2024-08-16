﻿using AKDEMIC.CORE.Constants;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers.InvestigacionFormativa
{
    [Route("api/maestrocategoriadocente")]
    [ApiController]
    public class ApplicationCategoriadocenteController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ApplicationCategoriadocenteController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> GetMaestroCategoriadocente()
        {
            var query = _context.MaestroCategoriadocentes             
                .OrderBy(x => x.nombre)
                .AsNoTracking();

            var result = await query
                .Select(x => new
                {
                    id = x.Id,
                    text = x.nombre,
                })
                .ToListAsync();

            return Ok(result);
        }
    }
}
