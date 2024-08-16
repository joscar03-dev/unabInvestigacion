using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages
{
    [Authorize]
    public class IndexpublicidadModel : PageModel
    {
        //private readonly ILogger<IndexpublicidadModel> _logger;
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly IEmailSenderService _emailSenderService;

        public IndexpublicidadModel(ILogger<IndexModel> logger,
            AkdemicContext context,
            IDataTablesService dataTablesService,
            IEmailSenderService emailSenderService
            )
        {
           // _logger = logger;
            _context = context;
            _dataTablesService = dataTablesService;
            _emailSenderService = emailSenderService;
        }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostSendProjectReportEmailAsync(Guid investigationProjectReportId)
        {
            var investigationProjectReportDb = await _context.InvestigationProjectReports
                .Where(x => x.Id == investigationProjectReportId)
                .FirstOrDefaultAsync();

            var investigationProjectReport = await _context.InvestigationProjectReports
                .Where(x => x.Id == investigationProjectReportId)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationProject.InvestigationConvocationPostulant.User.Email,
                    x.ExpirationDate,
                    ReportName = x.Name,
                    x.InvestigationProjectId                    
                })
                .FirstOrDefaultAsync();

            if (investigationProjectReport == null || investigationProjectReportDb == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (investigationProjectReport.ExpirationDate < DateTime.UtcNow)
            {
                return new BadRequestObjectResult("La fecha ya expiracion ya paso");
            }
            if (string.IsNullOrEmpty(investigationProjectReport.Email))
            {
                return new BadRequestObjectResult("No existe correo");
            }
            

            try
            {
                var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/investigador/proyectos/{investigationProjectReport.InvestigationProjectId}/detalle";
                await _emailSenderService.SendEmailProjectReport(Helpers.ConstantHelpers.PROJECT.NAME, investigationProjectReport.Email, GeneralConstants.GetApplicationRoute(GeneralConstants.Solution.TeacherInvestigation), callbackUrl);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("No se ha podido enviar el correo.");
            }

            investigationProjectReportDb.LastEmailSendedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();


            return new OkResult();
        }

        //Tablas

        public async Task<IActionResult> OnGetDatatableNotExpiredAsync()
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigationProjectReport, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.InvestigationProject.InvestigationConvocationPostulant.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InvestigationProject.InvestigationConvocationPostulant.User.Email;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ExpirationDate;
                    break;
                case "4":
                    orderByPredicate = (x) => x.ExpirationDate;
                    break;
                case "5":
                    orderByPredicate = (x) => x.LastEmailSendedDate;
                    break;
            }

            var query = _context.InvestigationProjectReports
                .Where(x => x.ExpirationDate >= DateTime.UtcNow && x.ReportUrl == null && (x.LastEmailSendedDate.HasValue ? (DateTime.UtcNow.Date != x.LastEmailSendedDate.Value.Date) : true))
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationProject.InvestigationConvocationPostulant.User.FullName,
                    x.InvestigationProject.InvestigationConvocationPostulant.User.Email,
                    ReportName = x.Name,
                    ExpirationDate = x.ExpirationDate.ToLocalDateFormat(),
                    LastEmailSendedDate = x.LastEmailSendedDate == null ? "--" : x.LastEmailSendedDate.ToLocalDateFormat(),
                    TimeRest = x.ExpirationDate.Subtract(DateTime.UtcNow).Days
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

        public async Task<IActionResult> OnGetDatatableExpiredAsync()
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigationProjectReport, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.InvestigationProject.InvestigationConvocationPostulant.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.InvestigationProject.InvestigationConvocationPostulant.User.Email;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.ExpirationDate;
                    break;

            }

            var query = _context.InvestigationProjectReports
                .Where(x => x.ExpirationDate <= DateTime.UtcNow && x.ReportUrl == null)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationProject.InvestigationConvocationPostulant.User.FullName,
                    x.InvestigationProject.InvestigationConvocationPostulant.User.Email,
                    ReportName = x.Name,
                    ExpirationDate = x.ExpirationDate.ToLocalDateFormat(),
                    TimeRest = x.ExpirationDate.Subtract(DateTime.UtcNow).Days
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
    }
}
