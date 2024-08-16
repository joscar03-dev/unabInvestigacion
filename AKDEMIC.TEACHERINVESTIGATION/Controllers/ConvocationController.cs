using System;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Constants;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/convocatorias")]
    [ApiController]
    public class ConvocationController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        public ConvocationController(
            AkdemicContext context
        )
        {
            _context = context;
        }
        [HttpGet("select/get")]
        public async Task<IActionResult> SearchConvocations(string term = null)
        {
            var query = _context.InvestigationConvocations.AsNoTracking();

            if (!string.IsNullOrEmpty(term))
            {
                string searchValue = term.Trim();

                query = query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()) ||
                x.Code.ToUpper().Contains(searchValue.ToUpper()));
            }

            var result = await query
                .OrderBy(x => x.Id)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.Code} - {x.Name}"
                })
                .Take(5)
                .ToListAsync();

            return Ok(result);

        }

        [HttpGet("progress/get")]
        public IActionResult ProgressConvocations()
        {
            var result = TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES
               .Select(x => new
               {
                   id = x.Key,
                   text = x.Value
               })
               .ToList();

            return Ok(result);

        }
        [HttpGet("reviews/get")]
        public IActionResult ReviewsConvocations()
        {

            var result = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES
               .Select(x => new
               {
                   id = x.Key,
                   text = x.Value
               })
               .ToList();

            return Ok(result);
        }
    }
}
