using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MySqlConnector;


namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestigacionInnovacion.InvestigacioninnovacionProyectoactividadPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
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
        Guid xIdOficina = Guid.Parse("edcd01ea-6404-11ee-b7b1-16d13ee00159");

        public async Task<IActionResult> OnGetDatatableAsync(string startDate, string endDate)
        {

            int flag = 2;
           // var data2 = _context.SpInvestigacionfomentoResumenactividades.FromSqlInterpolated($"call sp_investigacionfomento_resumenactividad(@fla={flag})").ToList();

            var parm1 = new MySqlParameter("_flag", "2");
          

            //var data2 = _context.SpInvestigacionfomentoResumenactividadesxdocentes.FromSqlInterpolated($"call sp_investigacionfomento_resumenactividadxdocente(@_flag={flag},@_idconvocatoria={searchconvocatoria2},@_idcocente={searchdocente2},@_idproyecto={searchconvocatoria2})").ToList();
            var data2 = _context.SpInvestigacionfomentoResumenactividades.FromSqlRaw("call sp_investigacionfomento_resumenactividad(@_flag)", parm1).ToList();


            var result2 = new DataTablesStructs.ReturnedData<object>
            {
                Data = data2,
                DrawCounter = 1,
                RecordsFiltered = 1,
                RecordsTotal = data2.Count(),
            };

            

            return new OkObjectResult(result2);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<InvestigationConvocation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.InvestigationConvocationPostulants
                    .Where(y => y.InvestigationConvocationId == x.Id)
                    .Count();
                    break;
                case "3":
                    orderByPredicate = (x) => x.InvestigationConvocationPostulants
                .Where(y => y.InvestigationConvocationId == x.Id && y.ProjectState == TeacherInvestigationConstants.ConvocationPostulant.ProjectState.ACCEPTED)
                .Count();
                    break;
            }

            var query = _context.InvestigationConvocations
                .AsNoTracking();

            if(!string.IsNullOrEmpty(startDate))
            {
                var startDateDT = ConvertHelpers.DatepickerToDatetime(startDate);
                query = query.Where(x => x.StartDate.Date >= startDateDT.Date);

            }

            if (!string.IsNullOrEmpty(endDate))
            {
                var endDateDT = ConvertHelpers.DatepickerToDatetime(endDate);
                query = query.Where(x => x.StartDate.Date <= endDateDT.Date);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    totalPostulations = x.InvestigationConvocationPostulants
                        .Where(y => y.InvestigationConvocationId == x.Id)
                        .Count(),
                    totalProjectApproveds = x.InvestigationConvocationPostulants
                        .Where(y => y.InvestigationConvocationId == x.Id && y.ProjectState == TeacherInvestigationConstants.ConvocationPostulant.ProjectState.ACCEPTED)
                        .Count()
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

        public void OnGet()
        {
        }
    }
}
