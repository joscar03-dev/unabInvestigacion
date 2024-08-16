using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.EvaluatorCommittee.Pages.InvestigationConvocationPostulantPage
{
    [Authorize(Roles = GeneralConstants.ROLES.EVALUATOR_COMMITTEE)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService
        )
        {
            _userManager = userManager;
            _dataTablesService = dataTablesService;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, Guid? investigationConvocationId = null, string searchStartDate = null, string searchEndDate = null, Guid? facultyId = null, int searchReviewState = -1, int searchProgressState = -1)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            var user = await _userManager.GetUserAsync(User);

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
                case "4":
                    orderByPredicate = (x) => x.FacultyText;
                    break;
                case "5":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.InvestigationConvocationPostulants
                .Where(x => x.InvestigationConvocation.EvaluatorCommitteeConvocations.Any(y => y.UserId == user.Id))
                .AsNoTracking();

            if (investigationConvocationId != null)
            {
                query = query.Where(x => x.InvestigationConvocationId == investigationConvocationId);
            }

            if (!string.IsNullOrEmpty(searchStartDate))
            {
                var startDate = ConvertHelpers.DatepickerToUtcDateTime(searchStartDate);
                query = query.Where(x => x.InvestigationConvocation.StartDate >= startDate);
            }
            if (facultyId != null)
            {
                query = query.Where(x => x.FacultyId == facultyId);
            }
            if (searchReviewState != -1)
            {
                query = query.Where(x => x.ReviewState == searchReviewState);
            }
            if (searchProgressState != -1)
            {
                query = query.Where(x => x.ProgressState == searchProgressState);
            }


            if (!string.IsNullOrEmpty(searchEndDate))
            {

                var endDate = ConvertHelpers.DatepickerToUtcDateTime(searchEndDate);
                query = query.Where(x => x.InvestigationConvocation.EndDate <= endDate);
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.InvestigationConvocation.Code.ToUpper().Contains(searchValue.ToUpper())
                                      || x.InvestigationConvocation.Name.ToUpper().Contains(searchValue.ToUpper())
                                      || x.ProjectTitle.ToUpper().Contains(searchValue.ToUpper()));
            }

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
                    Faculty = x.FacultyText,
                    CreatedAt = x.CreatedAt.ToDateFormat(),
                    PercentA = (x.InvestigationTypeId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight)
                             + (x.ExternalEntityId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight)
                             + (x.Budget == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetWeight)
                             + (x.InvestigationPatternId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight)
                             + (x.InvestigationAreaId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.AreaWeight)
                             + (x.FacultyId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyWeight)
                             + (x.CareerId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.CareerWeight)
                             + (x.ResearchCenterId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight)
                             + (x.FinancingInvestigationId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight)
                             + (x.PostulantExecutionPlaces.Count() == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight)
                             + (x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchLineCategoryRequirements
                                    .Where(y => x.PostulantResearchLines.Any(z => z.ResearchLine.ResearchLineCategoryId == y.ResearchLineCategoryId) && !y.Hidden)
                                    .Sum(y => y.Weight))
                             + (string.IsNullOrEmpty(x.ProjectTitle) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight)
                             + (string.IsNullOrEmpty(x.ProblemDescription) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight)
                             + (string.IsNullOrEmpty(x.GeneralGoal) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight)
                             + (string.IsNullOrEmpty(x.ProblemFormulation) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight)
                             + (string.IsNullOrEmpty(x.SpecificGoal) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight)
                             + (string.IsNullOrEmpty(x.Justification) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationWeight)
                             + (string.IsNullOrEmpty(x.TheoreticalFundament) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight)
                             + (string.IsNullOrEmpty(x.ProblemRecord) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight)
                             + (string.IsNullOrEmpty(x.Hypothesis) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisWeight)
                             + (string.IsNullOrEmpty(x.Variable) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.VariableWeight)
                             + (x.MethodologyTypeId == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight)
                             + (string.IsNullOrEmpty(x.MethodologyDescription) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight)
                             + (string.IsNullOrEmpty(x.Population) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationWeight)
                             + (string.IsNullOrEmpty(x.InformationCollectionTechnique) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight)
                             + (string.IsNullOrEmpty(x.AnalysisTechnique) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight)
                             + (string.IsNullOrEmpty(x.FieldWork) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight)
                             + (string.IsNullOrEmpty(x.ThesisDevelopment) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight)
                             + (string.IsNullOrEmpty(x.PublishedArticle) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight)
                             + (string.IsNullOrEmpty(x.BroadcastArticle) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight)
                             + (string.IsNullOrEmpty(x.ProcessDevelopment) ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight)
                             + (x.ProjectDuration == null ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight)
                             + (x.PostulantAnnexFiles.Count() > 0 ? x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight : 0)
                             + (x.PostulantTeamMemberUsers.Count() > 0 ? x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight : 0)
                             + (x.PostulantExternalMembers.Count() == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight)
                             + (_context.InvestigationAnswerByUsers
                                .Count(y => y.UserId == x.UserId && y.InvestigationQuestion.InvestigationConvocationRequirement.InvestigationConvocation.Id == x.InvestigationConvocationId) == 0 ? 0 : x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsWeight),
                    State = x.InvestigationConvocation.State,
                    ProgressState = x.ProgressState,
                    ReviewState = x.ReviewState,
                    ProgressStateText = TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES.ContainsKey(x.ProgressState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES[x.ProgressState] : "",
                    ReviewStateText = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[x.ReviewState] : "",




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
