using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Report.ViewModels.InvestigationProjectViewModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestigationProjectPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
    GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
    GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
    GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class DetailModel : PageModel
    {
        private readonly IConverter _dinkConverter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public DetailModel(
    IConverter dinkConverter,
    IWebHostEnvironment hostingEnvironment,
    IViewRenderService viewRenderService,
    AkdemicContext context,
    IDataTablesService dataTablesService
)
        {
            _dinkConverter = dinkConverter;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _context = context;
            _dataTablesService = dataTablesService;
        }

        [BindProperty]
        public InvestigationProjectViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid investigationProjectId)
        {
            var investigationProject = await _context.InvestigationProjects
                .Where(x => x.Id == investigationProjectId)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationConvocationPostulant.CareerText,
                    x.InvestigationConvocationPostulant.Budget,
                    x.InvestigationConvocationPostulant.ProjectState,
                    x.GeneralGoal,
                    x.InvestigationConvocationPostulant.ProjectTitle,
                    x.SpecificGoal,
                    x.GanttDiagramUrl,
                    x.ExecutionAddress,
                    x.InvestigationProjectTypeId,
                    InvestigationProjectType = x.InvestigationProjectType.Name ?? "",
                    x.InvestigationConvocationPostulant.User.FullName,
                    x.InvestigationConvocationPostulantId,
                    FinancingInvestigation = x.InvestigationConvocationPostulant.FinancingInvestigation.Name ?? ""
                })
                .FirstOrDefaultAsync();

            if (investigationProject == null)
                return RedirectToPage("Index");

            Input = new InvestigationProjectViewModel
            {
                InvestigationProjectId = investigationProject.Id,
                CareerText = investigationProject.CareerText,
                Budget = investigationProject.Budget,
                ProjectState = TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES.ContainsKey(investigationProject.ProjectState)?
                TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES[investigationProject.ProjectState]: "",
                GeneralGoal = investigationProject.GeneralGoal,
                SpecificGoal = investigationProject.SpecificGoal,
                GanttDiagramUrl = investigationProject.GanttDiagramUrl,
                ExecutionAddress = investigationProject.ExecutionAddress,
                InvestigationProjectType = investigationProject.InvestigationProjectType,
                FullName = investigationProject.FullName,
                ProjectTitle = investigationProject.ProjectTitle,
                InvestigationConvocationPostulantId = investigationProject.InvestigationConvocationPostulantId,
                FinancingInvestigation = investigationProject.FinancingInvestigation
            };

            return Page();
        }
        public async Task<IActionResult> OnGetExpenseDatatableAsync(Guid investigationProjectId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.InvestigationProjectExpense, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Amount;
                    break;
                case "2":
                    orderByPredicate = (x) => x.InvestigationProjectTask.InvestigationProject.InvestigationConvocationPostulant.FinancingInvestigation.Name;
                    break;
            }

            var query = _context.InvestigationProjectExpenses
                .Where(x => x.InvestigationProjectTask.InvestigationProjectId == investigationProjectId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.Description,
                    x.Amount,
                    FinancingInvestigation = x.InvestigationProjectTask.InvestigationProject.InvestigationConvocationPostulant.FinancingInvestigation.Name ?? ""
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

        public async Task<IActionResult> OnGetProjectTeamMemberDatatableAsync(Guid investigationProjectId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.InvestigationProjectTeamMember, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.CvFilePath;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Objectives;
                    break;

            }

            var query = _context.InvestigationProjectTeamMembers
                .Where(x => x.InvestigationProjectId == investigationProjectId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.User.FullName,
                    x.User.CteVitaeConcytecUrl,
                    x.Objectives

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
