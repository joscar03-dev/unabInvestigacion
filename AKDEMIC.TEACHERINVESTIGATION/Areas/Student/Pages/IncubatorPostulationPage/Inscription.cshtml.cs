using AKDEMIC.CORE.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Student.ViewModels.IncubatorPostulationViewModels;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using System.Linq.Expressions;
using static AKDEMIC.CORE.Constants.Systems.TeacherInvestigationConstants;
using DocumentFormat.OpenXml.Office2010.Excel;
using static AKDEMIC.CORE.Structs.Select2Structs;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Student.Pages.IncubatorPostulationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.STUDENTS)]
    public class InscriptionModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public InscriptionModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
        }
        public Guid IncubatorPostulationId { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return RedirectToPage("Index");

            IncubatorPostulationId = incubatorPostulation.Id;

            return Page();
        }

        #region GeneralInformation
        public async Task<IActionResult> OnGetGeneralInformationLoadAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId && x.UserId == user.Id)
                .Select(x => new
                {
                    IncubatorConvocationCode = x.IncubatorConvocation.Code,
                    IncubatorConvocationName = x.IncubatorConvocation.Name,
                    x.Title,
                    x.Budget,
                    x.MonthDuration,
                    x.GeneralGoals,
                    x.DepartmentId,
                    x.ProvinceId,
                    x.DistrictId
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
                incubatorPostulation.DepartmentId,
                incubatorPostulation.ProvinceId,
                incubatorPostulation.DistrictId,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostGeneralInformationSaveAsync(InscriptionGeneralInformationViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            incubatorPostulation.Title = viewModel.Title;
            incubatorPostulation.Budget = viewModel.Budget;
            incubatorPostulation.MonthDuration = viewModel.MonthDuration;
            incubatorPostulation.GeneralGoals = viewModel.GeneralGoals;
            incubatorPostulation.DepartmentId = viewModel.DepartmentId;
            incubatorPostulation.DepartmentText = viewModel.DepartmentText;
            incubatorPostulation.ProvinceId = viewModel.ProvinceId;
            incubatorPostulation.ProvinceText = viewModel.ProvinceText;
            incubatorPostulation.DistrictId = viewModel.DistrictId;
            incubatorPostulation.DistrictText = viewModel.DistrictText;
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetSpecificGoalDatatableAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

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
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
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

        public async Task<IActionResult> OnPostSpecificGoalAddAsync(SpecificGoalAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(viewModel.Description))
                return new BadRequestObjectResult("Debe especificar un objetivo especifico");

            if (viewModel.OrderNumber < 0)
                return new BadRequestObjectResult("El orden no puede ser negativo");

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var incubatorPostulationSpecificGoal = new IncubatorPostulationSpecificGoal
            {
                IncubatorPostulationId = incubatorPostulation.Id,
                Description = viewModel.Description,
                OrderNumber = viewModel.OrderNumber,
            };

            await _context.IncubatorPostulationSpecificGoals.AddAsync(incubatorPostulationSpecificGoal);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostSpecificGoalEditAsync(SpecificGoalEditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var incubatorPostulationSpecificGoal = await _context.IncubatorPostulationSpecificGoals
                .Where(x => x.Id == viewModel.Id && x.IncubatorPostulationId == viewModel.IncubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(viewModel.Description))
                return new BadRequestObjectResult("Debe especificar un objetivo especifico");

            if (viewModel.OrderNumber < 0)
                return new BadRequestObjectResult("El orden no puede ser negativo");

            if (incubatorPostulationSpecificGoal == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            incubatorPostulationSpecificGoal.Description = viewModel.Description;
            incubatorPostulationSpecificGoal.OrderNumber = viewModel.OrderNumber;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetSpecificGoalLoadAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulationSpecificGoal = await _context.IncubatorPostulationSpecificGoals
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulationSpecificGoal == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var result = new
            {
                incubatorPostulationSpecificGoal.Id,
                incubatorPostulationSpecificGoal.Description,
                incubatorPostulationSpecificGoal.OrderNumber
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostSpecificGoalDeleteAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulationSpecificGoal = await _context.IncubatorPostulationSpecificGoals
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulationSpecificGoal == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorPostulationSpecificGoals.Remove(incubatorPostulationSpecificGoal);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #endregion

        #region InvestigationTeam

        public async Task<IActionResult> OnGetInvestigationTeamLoadAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId && x.UserId == user.Id)
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

        public async Task<IActionResult> OnPostInvestigationTeamSaveAsync(InvestigationTeamAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if(viewModel.AdviserId == null || viewModel.CoAdviserId == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            incubatorPostulation.AdviserId = viewModel.AdviserId;
            incubatorPostulation.CoAdviserId = viewModel.CoAdviserId;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetTeamMemberDatatableAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

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
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
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

        public async Task<IActionResult> OnPostTeamMemberAddAsync(TeamMemberAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var teamMember = new IncubatorPostulationTeamMember
            {
                IncubatorPostulationId = viewModel.IncubatorPostulationId,
                UserName = viewModel.UserName,
                PaternalSurname = viewModel.PaternalSurName,
                MaternalSurname = viewModel.MaternalSurName,
                Name = viewModel.Name,
                Sex = viewModel.Sex,
                CareerText = viewModel.CareerText,
            };

            await _context.IncubatorPostulationTeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();


            return new OkResult();
        }
        public async Task<IActionResult> OnPostTeamMemberDeleteAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulationTeamMember = await _context.IncubatorPostulationTeamMembers
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulationTeamMember == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorPostulationTeamMembers.Remove(incubatorPostulationTeamMember);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        #endregion

        #region BusinessIdea
        public async Task<IActionResult> OnGetBusinessIdeaLoadAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId && x.UserId == user.Id)
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

        public async Task<IActionResult> OnPostBusinessIdeaSaveAsync(BusinessIdeaViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            incubatorPostulation.BusinessIdeaDescription = viewModel.BusinessIdeaDescription;
            incubatorPostulation.CompetitiveAdvantages = viewModel.CompetitiveAdvantages;
            incubatorPostulation.MarketStudy = viewModel.MarketStudy;
            incubatorPostulation.MarketingPlan = viewModel.MarketingPlan;
            incubatorPostulation.Resources = viewModel.Resources;
            incubatorPostulation.PotentialStrategicPartners = viewModel.PotentialStrategicPartners;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #endregion

        #region BusinessPlan
        public async Task<IActionResult> OnGetBusinessPlanLoadAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId && x.UserId == user.Id)
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

        public async Task<IActionResult> OnPostBusinessPlanSaveAsync(BusinessPlanViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            incubatorPostulation.Mission = viewModel.Mission;
            incubatorPostulation.ProductDescription = viewModel.ProductDescription;
            incubatorPostulation.TechnicalViability = viewModel.TechnicalViability;
            incubatorPostulation.EconomicViability = viewModel.EconomicViability;
            incubatorPostulation.MerchandisingPlan = viewModel.MerchandisingPlan;
            incubatorPostulation.Breakeven = viewModel.Breakeven;
            incubatorPostulation.AffectationLevel = viewModel.AffectationLevel;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        #endregion

        #region ScheduleAndBudget

        public async Task<IActionResult> OnPostActivityAddAsync(ActivityAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var specificGoal = await _context.IncubatorPostulationSpecificGoals
                .Where(x => x.Id == viewModel.IncubatorPostulationSpecificGoalId && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (specificGoal == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (string.IsNullOrEmpty(viewModel.Description))
                return new BadRequestObjectResult("Debe escribir una descripción para la actividad");

            var incubatorPostulationActivity = new IncubatorPostulationActivity
            {
                IncubatorPostulationSpecificGoalId = specificGoal.Id,
                Description = viewModel.Description,
                OrderNumber = viewModel.OrderNumber
            };

            await _context.IncubatorPostulationActivities.AddAsync(incubatorPostulationActivity);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

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

        public async Task<IActionResult> OnPostActivityEditAsync(ActivityEditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var incubatorPostulationActivity = await _context.IncubatorPostulationActivities
                .Where(x => x.Id == viewModel.Id && x.IncubatorPostulationSpecificGoalId == viewModel.IncubatorPostulationSpecificGoalId && x.IncubatorPostulationSpecificGoal.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulationActivity == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (string.IsNullOrEmpty(viewModel.Description))
                return new BadRequestObjectResult("Debe escribir una descripción para la actividad");

            incubatorPostulationActivity.Description = viewModel.Description;
            incubatorPostulationActivity.OrderNumber = viewModel.OrderNumber;


            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostActivityDeleteAsync(Guid id)
        {
            var incubatorPostulationActivity = await _context.IncubatorPostulationActivities
                .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (incubatorPostulationActivity == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorPostulationActivities.Remove(incubatorPostulationActivity);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostActivityMonthSaveAsync(ActivityMonthSaveViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var activityMonths = await _context.IncubatorPostulationActivityMonths
                .Where(x => x.IncubatorPostulationActivityId == viewModel.IncubatorPostulationActivityId)
                .ToListAsync();

            if (activityMonths == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (activityMonths.Count > 0)
            {
                _context.IncubatorPostulationActivityMonths.RemoveRange(activityMonths);
                await _context.SaveChangesAsync();
            }

            if (viewModel.Months != null)
            {
                if (viewModel.Months.Count > 0)
                {
                    for (int i = 0; i < viewModel.Months.Count; i++)
                    {
                        var incubatorPostulationActivityMonth = new IncubatorPostulationActivityMonth
                        {
                            IncubatorPostulationActivityId = viewModel.IncubatorPostulationActivityId,
                            MonthNumber = viewModel.Months[i]
                        };

                        await _context.IncubatorPostulationActivityMonths.AddAsync(incubatorPostulationActivityMonth);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            return new OkResult();
        }

        public async Task<IActionResult> OnGetSchedulePageAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var viewModel = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId && x.UserId == user.Id)
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


            return Partial("IncubatorPostulationPage/Partials/_ScheduleSectionPartial", viewModel);
        }

        public async Task<IActionResult> OnGetEquipmentExpenseDatatableAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            var query = _context.IncubatorEquipmentExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
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

        public async Task<IActionResult> OnPostEquipmentExpenseAddAsync(EquipmentExpenseAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();



            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var incubatorEquipmentExpense = new IncubatorEquipmentExpense
            {
                IncubatorPostulationId = incubatorPostulation.Id,
                ExpenseCode = viewModel.ExpenseCode,
                Description = viewModel.Description,
                MeasureUnit = viewModel.MeasureUnit,
                Quantity = viewModel.Quantity,
                UnitPrice = viewModel.UnitPrice,
                ActivityJustification = viewModel.ActivityJustification,
            };

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;
            
            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene "+ incubatorPostulation.Budget.ToString("0.00"));

            await _context.IncubatorEquipmentExpenses.AddAsync(incubatorEquipmentExpense);
            await _context.SaveChangesAsync();


            return new OkResult();
        }

        public async Task<IActionResult> OnPostEquipmentExpenseEditAsync(EquipmentExpenseEditViewModel viewModel)
        {

            var user = await _userManager.GetUserAsync(User);

            var incubatorEquipmentExpense = await _context.IncubatorEquipmentExpenses
                .Where(x => x.Id == viewModel.Id && x.IncubatorPostulationId == viewModel.IncubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            var incubatorPostulation = await _context.IncubatorPostulations
    .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
    .FirstOrDefaultAsync();

            if (incubatorEquipmentExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            incubatorEquipmentExpense.ExpenseCode = viewModel.ExpenseCode;
            incubatorEquipmentExpense.Description = viewModel.Description;
            incubatorEquipmentExpense.MeasureUnit = viewModel.MeasureUnit;
            incubatorEquipmentExpense.Quantity = viewModel.Quantity;
            incubatorEquipmentExpense.UnitPrice = viewModel.UnitPrice;
            incubatorEquipmentExpense.ActivityJustification = viewModel.ActivityJustification;

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
        .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id && x.Id != viewModel.Id)
        .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetEquipmentExpenseLoadAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorEquipmentExpense = await _context.IncubatorEquipmentExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorEquipmentExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var result = new
            {
                incubatorEquipmentExpense.Id,
                incubatorEquipmentExpense.ExpenseCode,
                incubatorEquipmentExpense.Description,
                incubatorEquipmentExpense.MeasureUnit,
                incubatorEquipmentExpense.Quantity,
                incubatorEquipmentExpense.UnitPrice,
                incubatorEquipmentExpense.ActivityJustification
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEquipmentExpenseDeleteAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorEquipmentExpense = await _context.IncubatorEquipmentExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorEquipmentExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorEquipmentExpenses.Remove(incubatorEquipmentExpense);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetSuppliesExpenseDatatableAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            var query = _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
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

        public async Task<IActionResult> OnPostSuppliesExpenseAddAsync(SupplyExpenseAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");



            var incubatorSuppliesExpense = new IncubatorSuppliesExpense
            {
                IncubatorPostulationId = incubatorPostulation.Id,
                ExpenseCode = viewModel.ExpenseCode,
                Description = viewModel.Description,
                MeasureUnit = viewModel.MeasureUnit,
                Quantity = viewModel.Quantity,
                UnitPrice = viewModel.UnitPrice,
                ActivityJustification = viewModel.ActivityJustification,
            };

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));

            await _context.IncubatorSuppliesExpenses.AddAsync(incubatorSuppliesExpense);
            await _context.SaveChangesAsync();


            return new OkResult();
        }

        public async Task<IActionResult> OnPostSuppliesExpenseEditAsync(SupplyExpenseEditViewModel viewModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var incubatorSuppliesExpense = await _context.IncubatorSuppliesExpenses
                .Where(x => x.Id == viewModel.Id && x.IncubatorPostulationId == viewModel.IncubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorSuppliesExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            incubatorSuppliesExpense.ExpenseCode = viewModel.ExpenseCode;
            incubatorSuppliesExpense.Description = viewModel.Description;
            incubatorSuppliesExpense.MeasureUnit = viewModel.MeasureUnit;
            incubatorSuppliesExpense.Quantity = viewModel.Quantity;
            incubatorSuppliesExpense.UnitPrice = viewModel.UnitPrice;
            incubatorSuppliesExpense.ActivityJustification = viewModel.ActivityJustification;

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id && x.Id != viewModel.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetSuppliesExpenseLoadAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorSuppliesExpense = await _context.IncubatorSuppliesExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorSuppliesExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var result = new
            {
                incubatorSuppliesExpense.Id,
                incubatorSuppliesExpense.ExpenseCode,
                incubatorSuppliesExpense.Description,
                incubatorSuppliesExpense.MeasureUnit,
                incubatorSuppliesExpense.Quantity,
                incubatorSuppliesExpense.UnitPrice,
                incubatorSuppliesExpense.ActivityJustification
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostSuppliesExpenseDeleteAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorSuppliesExpense = await _context.IncubatorSuppliesExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorSuppliesExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorSuppliesExpenses.Remove(incubatorSuppliesExpense);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetThirdPartyServiceExpenseDatatableAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();


            var query = _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
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

        public async Task<IActionResult> OnPostThirdPartyServiceExpenseAddAsync(ThirdPartyServiceExpenseAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var incubatorThirdPartyServiceExpense = new IncubatorThirdPartyServiceExpense
            {
                IncubatorPostulationId = incubatorPostulation.Id,
                ExpenseCode = viewModel.ExpenseCode,
                Description = viewModel.Description,
                MeasureUnit = viewModel.MeasureUnit,
                Quantity = viewModel.Quantity,
                UnitPrice = viewModel.UnitPrice,
                ActivityJustification = viewModel.ActivityJustification,
            };

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));

            await _context.IncubatorThirdPartyServiceExpenses.AddAsync(incubatorThirdPartyServiceExpense);
            await _context.SaveChangesAsync();


            return new OkResult();
        }

        public async Task<IActionResult> OnPostThirdPartyServiceExpenseEditAsync(ThirdPartyServiceExpenseEditViewModel viewModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var incubatorThirdPartyServicesExpense = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.Id == viewModel.Id && x.IncubatorPostulationId == viewModel.IncubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();
            if (incubatorThirdPartyServicesExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            incubatorThirdPartyServicesExpense.ExpenseCode = viewModel.ExpenseCode;
            incubatorThirdPartyServicesExpense.Description = viewModel.Description;
            incubatorThirdPartyServicesExpense.MeasureUnit = viewModel.MeasureUnit;
            incubatorThirdPartyServicesExpense.Quantity = viewModel.Quantity;
            incubatorThirdPartyServicesExpense.UnitPrice = viewModel.UnitPrice;
            incubatorThirdPartyServicesExpense.ActivityJustification = viewModel.ActivityJustification;

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id && x.Id != viewModel.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));


            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetThirdPartyServiceExpenseLoadAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorThirdPartyServiceExpense = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorThirdPartyServiceExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var result = new
            {
                incubatorThirdPartyServiceExpense.Id,
                incubatorThirdPartyServiceExpense.ExpenseCode,
                incubatorThirdPartyServiceExpense.Description,
                incubatorThirdPartyServiceExpense.MeasureUnit,
                incubatorThirdPartyServiceExpense.Quantity,
                incubatorThirdPartyServiceExpense.UnitPrice,
                incubatorThirdPartyServiceExpense.ActivityJustification
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostThirdPartyServiceExpenseDeleteAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorThirdPartyServiceExpense = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorThirdPartyServiceExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorThirdPartyServiceExpenses.Remove(incubatorThirdPartyServiceExpense);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetOtherExpenseDatatableAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();


            var query = _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
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

        public async Task<IActionResult> OnPostOtherExpenseAddAsync(OtherExpenseAddViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var incubatorOtherExpense = new IncubatorOtherExpense
            {
                IncubatorPostulationId = incubatorPostulation.Id,
                ExpenseCode = viewModel.ExpenseCode,
                Description = viewModel.Description,
                MeasureUnit = viewModel.MeasureUnit,
                Quantity = viewModel.Quantity,
                UnitPrice = viewModel.UnitPrice,
                ActivityJustification = viewModel.ActivityJustification,
            };

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));

            await _context.IncubatorOtherExpenses.AddAsync(incubatorOtherExpense);
            await _context.SaveChangesAsync();


            return new OkResult();
        }

        public async Task<IActionResult> OnPostOtherExpenseEditAsync(OtherExpenseEditViewModel viewModel)
        {

            var user = await _userManager.GetUserAsync(User);
            var incubatorOtherExpense = await _context.IncubatorOtherExpenses
                .Where(x => x.Id == viewModel.Id && x.IncubatorPostulationId == viewModel.IncubatorPostulationId && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();
            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == viewModel.IncubatorPostulationId && x.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorOtherExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            incubatorOtherExpense.ExpenseCode = viewModel.ExpenseCode;
            incubatorOtherExpense.Description = viewModel.Description;
            incubatorOtherExpense.MeasureUnit = viewModel.MeasureUnit;
            incubatorOtherExpense.Quantity = viewModel.Quantity;
            incubatorOtherExpense.UnitPrice = viewModel.UnitPrice;
            incubatorOtherExpense.ActivityJustification = viewModel.ActivityJustification;

            var nuevoGasto = viewModel.Quantity * viewModel.UnitPrice;

            var totalGastoBienes = await _context.IncubatorEquipmentExpenses
                    .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                    .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoOtros = await _context.IncubatorOtherExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id && x.Id != viewModel.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoInsumos = await _context.IncubatorSuppliesExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastoServicioTerceros = await _context.IncubatorThirdPartyServiceExpenses
                .Where(x => x.IncubatorPostulationId == incubatorPostulation.Id && x.IncubatorPostulation.UserId == user.Id)
                .SumAsync(x => x.Quantity * x.UnitPrice);

            var totalGastos = totalGastoBienes + totalGastoInsumos + totalGastoOtros + totalGastoServicioTerceros;

            if (incubatorPostulation.Budget < nuevoGasto + totalGastos)
                return new BadRequestObjectResult("Se ha pasado del presupuesto que tiene " + incubatorPostulation.Budget.ToString("0.00"));

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnGetOtherExpenseLoadAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorOtherExpense = await _context.IncubatorOtherExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorOtherExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            var result = new
            {
                incubatorOtherExpense.Id,
                incubatorOtherExpense.ExpenseCode,
                incubatorOtherExpense.Description,
                incubatorOtherExpense.MeasureUnit,
                incubatorOtherExpense.Quantity,
                incubatorOtherExpense.UnitPrice,
                incubatorOtherExpense.ActivityJustification
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostOtherExpenseDeleteAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            var incubatorOtherExpense = await _context.IncubatorOtherExpenses
                .Where(x => x.Id == id && x.IncubatorPostulation.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (incubatorOtherExpense == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            _context.IncubatorOtherExpenses.Remove(incubatorOtherExpense);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        #endregion



    }
}
