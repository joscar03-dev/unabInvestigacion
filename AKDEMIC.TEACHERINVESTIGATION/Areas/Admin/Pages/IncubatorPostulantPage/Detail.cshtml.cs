using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.IncubatorPostulantViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.IncubatorPostulantPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public DetailModel(
    AkdemicContext context,
    IDataTablesService dataTablesService)
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        public IncubatorPostulantDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorPostulantId)
        {

            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulantId)
                .Select(x => new IncubatorPostulantDetailViewModel
                {
                    Id = x.Id,
                    ConvocationCode = x.IncubatorConvocation.Code,
                    ConvocationName = x.IncubatorConvocation.Name,
                    PostulantFullName = x.User.FullName,
                    PostulantCode = x.User.UserName,
                    Title = x.Title,
                    GeneralGoals = x.GeneralGoals,
                    MonthDuration = x.MonthDuration,
                    Budget = x.Budget,
                    DepartmentText = x.DepartmentText,
                    ProvinceText = x.ProvinceText,
                    DistrictText = x.DistrictText
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return RedirectToPage("Index");

            Input = incubatorPostulation;

            return Page();
        }

        public async Task<IActionResult> OnGetAnnexesDatatableAsync(Guid incubatorPostulantId)
        {


            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorPostulationAnnex, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.IncubatorConvocationAnnex.Code;
                    break;

                case "1":
                    orderByPredicate = (x) => x.IncubatorConvocationAnnex.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.FilePath;
                    break;
            }

            var query = _context.IncubatorPostulationAnnexes
                .Include(x => x.IncubatorConvocationAnnex)
                .Include(x => x.IncubatorPostulation)
                .Include(x => x.IncubatorPostulation.User)
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorConvocationAnnex.Code,
                    x.IncubatorConvocationAnnex.Name,
                    x.FilePath
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

        #region GeneralInformation
        public async Task<IActionResult> OnGetGeneralInformationLoadAsync(Guid incubatorPostulantId)
        {
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulantId)
                .Select(x => new
                {
                    IncubatorConvocationCode = x.IncubatorConvocation.Code,
                    IncubatorConvocationName = x.IncubatorConvocation.Name,
                    x.Title,
                    x.Budget,
                    x.MonthDuration,
                    x.GeneralGoals,
                    x.DepartmentText,
                    x.ProvinceText,
                    x.DistrictText
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("No se ha podido cargar la información");

            var result = new
            {
                incubatorPostulation.IncubatorConvocationCode,
                incubatorPostulation.IncubatorConvocationName,
                incubatorPostulation.Title,
                incubatorPostulation.Budget,
                incubatorPostulation.MonthDuration,
                incubatorPostulation.GeneralGoals,
                incubatorPostulation.DepartmentText,
                incubatorPostulation.ProvinceText,
                incubatorPostulation.DistrictText,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetSpecificGoalDatatableAsync(Guid incubatorPostulantId)
        {

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorPostulationSpecificGoal, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.OrderNumber;
                    break;
            }

            var query = _context.IncubatorPostulationSpecificGoals
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Description,
                    x.OrderNumber
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


        #endregion

        #region InvestigationTeam

        public async Task<IActionResult> OnGetInvestigationTeamLoadAsync(Guid incubatorPostulantId)
        {
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulantId)
                .Select(x => new
                {
                    x.AdviserId,
                    x.CoAdviserId,
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("No se ha podido cargar la información");

            var result = new
            {
                incubatorPostulation.AdviserId,
                incubatorPostulation.CoAdviserId,
            };

            return new OkObjectResult(result);
        }



        public async Task<IActionResult> OnGetTeamMemberDatatableAsync(Guid incubatorPostulantId)
        {

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorPostulationTeamMember, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CareerText;
                    break;
            }
            var query = _context.IncubatorPostulationTeamMembers
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.CareerText,
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

        #endregion

        #region BusinessIdea
        public async Task<IActionResult> OnGetBusinessIdeaLoadAsync(Guid incubatorPostulantId)
        {

            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulantId)
                .Select(x => new
                {
                    x.BusinessIdeaDescription,
                    x.CompetitiveAdvantages,
                    x.MarketStudy,
                    x.MarketingPlan,
                    x.Resources,
                    x.PotentialStrategicPartners
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("No se ha podido cargar la información");

            var result = new
            {
                incubatorPostulation.BusinessIdeaDescription,
                incubatorPostulation.CompetitiveAdvantages,
                incubatorPostulation.MarketStudy,
                incubatorPostulation.MarketingPlan,
                incubatorPostulation.Resources,
                incubatorPostulation.PotentialStrategicPartners
            };

            return new OkObjectResult(result);
        }

        #endregion

        #region BusinessPlan
        public async Task<IActionResult> OnGetBusinessPlanLoadAsync(Guid incubatorPostulantId)
        {
         
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulantId)
                .Select(x => new
                {
                    x.Mission,
                    x.ProductDescription,
                    x.TechnicalViability,
                    x.EconomicViability,
                    x.MerchandisingPlan,
                    x.Breakeven,
                    x.AffectationLevel
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("No se ha podido cargar la información");

            var result = new
            {

                incubatorPostulation.Mission,
                incubatorPostulation.ProductDescription,
                incubatorPostulation.TechnicalViability,
                incubatorPostulation.EconomicViability,
                incubatorPostulation.MerchandisingPlan,
                incubatorPostulation.Breakeven,
                incubatorPostulation.AffectationLevel
            };

            return new OkObjectResult(result);
        }

        #endregion

        #region ScheduleAndBudget

        public async Task<IActionResult> OnGetActivityLoadAsync(Guid id)
        {
            var incubatorPostulationActivity = await _context.IncubatorPostulationActivities
                .Where(x => x.Id == id)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorPostulationSpecificGoalId,
                    x.Description,
                    x.OrderNumber,
                    SpecificGoalDescription = x.IncubatorPostulationSpecificGoal.Description
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulationActivity == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var result = new
            {
                incubatorPostulationActivity.Id,
                incubatorPostulationActivity.IncubatorPostulationSpecificGoalId,
                incubatorPostulationActivity.SpecificGoalDescription,
                incubatorPostulationActivity.Description,
                incubatorPostulationActivity.OrderNumber,
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetSchedulePageAsync(Guid incubatorPostulantId)
        {

            var viewModel = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulantId)
                .Select(x => new ScheduleViewModels
                {
                    MonthDuration = x.MonthDuration,
                    SpecificGoals = x.IncubatorPostulationSpecificGoals
                        .Select(y => new SpecificGoalViewModel
                        {
                            Id = y.Id,
                            Description = y.Description,
                            OrderNumber = y.OrderNumber,
                            Activities = y.IncubatorPostulationActivities
                                .Select(z => new ActivityViewModel
                                {
                                    Id = z.Id,
                                    IncubatorPostulationSpecificGoalId = z.IncubatorPostulationSpecificGoalId,
                                    Description = z.Description,
                                    OrderNumber = z.OrderNumber,
                                    ActivityMonths = z.IncubatorPostulationActivityMonths
                                        .Select(w => new ActivityMonthViewModel
                                        {
                                            MonthNumber = w.MonthNumber,
                                            IncubatorPostulationActivityId = w.IncubatorPostulationActivityId
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList()
                })
            .FirstOrDefaultAsync();


            return Partial("IncubatorPostulantPage/Partials/_ScheduleSectionPartial", viewModel);
        }

        public async Task<IActionResult> OnGetEquipmentExpenseDatatableAsync(Guid incubatorPostulantId)
        {

            var sentParameters = _dataTablesService.GetSentParameters();

            var query = _context.IncubatorEquipmentExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.ExpenseCode,
                    x.Description,
                    x.MeasureUnit,
                    x.Quantity,
                    x.UnitPrice,
                    Total = x.Quantity * x.UnitPrice,
                    x.ActivityJustification,
                })
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


        public async Task<IActionResult> OnGetSuppliesExpenseDatatableAsync(Guid incubatorPostulantId)
        {

            var sentParameters = _dataTablesService.GetSentParameters();

            var query = _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.ExpenseCode,
                    x.Description,
                    x.MeasureUnit,
                    x.Quantity,
                    x.UnitPrice,
                    Total = x.Quantity * x.UnitPrice,
                    x.ActivityJustification,
                })
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

        public async Task<IActionResult> OnGetThirdPartyServiceExpenseDatatableAsync(Guid incubatorPostulantId)
        {

            var sentParameters = _dataTablesService.GetSentParameters();


            var query = _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.ExpenseCode,
                    x.Description,
                    x.MeasureUnit,
                    x.Quantity,
                    x.UnitPrice,
                    Total = x.Quantity * x.UnitPrice,
                    x.ActivityJustification,
                })
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

        public async Task<IActionResult> OnGetOtherExpenseDatatableAsync(Guid incubatorPostulantId)
        {

            var sentParameters = _dataTablesService.GetSentParameters();


            var query = _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    x.Id,
                    x.ExpenseCode,
                    x.Description,
                    x.MeasureUnit,
                    x.Quantity,
                    x.UnitPrice,
                    Total = x.Quantity * x.UnitPrice,
                    x.ActivityJustification,
                })
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
        #endregion
    }
}
