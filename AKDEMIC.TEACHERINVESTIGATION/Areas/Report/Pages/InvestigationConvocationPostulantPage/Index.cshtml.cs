using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestigationConvocationPostulantPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService
        )
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync()
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationConvocationPostulant, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = (x) => x.InvestigationConvocation.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InvestigationConvocation.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.ProjectTitle;
                    break;
                case "3":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
            }

            var query = _context.InvestigationConvocationPostulants.AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.Id,
                    Code = x.InvestigationConvocation.Code,
                    Name = x.InvestigationConvocation.Name,
                    ProjectTitle = x.ProjectTitle,
                    FullName = x.User.FullName,
                    ReviewStateText = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[x.ReviewState] : "",
                    ReviewState = x.ReviewState
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetChartAsync()
        {
            var postulantsGrouped = await _context.InvestigationConvocationPostulants
                .GroupBy(x => x.ReviewState)
                .Select(x => new
                {
                    reviewState = x.Key,
                    count = x.Count()
                })
                .ToListAsync();


            //Estados de revisión
            var postulantReviewStates = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES
                .Select(x => new
                {
                    name = x.Value,
                    count = postulantsGrouped.Where(y => y.reviewState == x.Key).Select(x => x.count).FirstOrDefault()
                })
                .ToList();         

            var result = new
            {
                categories = postulantReviewStates.Select(x => x.name).ToList(),
                data = postulantReviewStates.Select(x => x.count).ToList()
            };

            return new ObjectResult(result);
        }
    }
}
