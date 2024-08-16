using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.Pages.Postulant
{
    [Authorize(Roles = GeneralConstants.ROLES.EXTERNAL_EVALUATOR)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;

        public DetailModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IDataTablesService dataTablesService
        )
        {
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
        }

        [BindProperty]
        public InvestigationConvocationPostulantViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(Guid investigationConvocationPostulantId)
        {
            var investigationConvocationPostulant = await _context.InvestigationConvocationPostulants
                .Include(x => x.InvestigationConvocation.InvestigationConvocationRequirement)
                .Include(x => x.InvestigationType)
                .Include(x => x.ExternalEntity)
                .Include(x => x.ResearchCenter)
                .Include(x => x.InvestigationPattern)
                .Include(x => x.InvestigationArea)
                .Include(x => x.FinancingInvestigation)
                .Include(x => x.MethodologyType)
                .Include(x => x.PostulantRubricQualifications)
                .Where(x => x.Id == investigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);

            if (investigationConvocationPostulant == null)
            {
                return RedirectToPage("/Postulant/Index");
            }

            var evaluator = await _context.InvestigationConvocationEvaluators
                .AnyAsync(x => x.UserId == user.Id && x.InvestigationConvocationId == investigationConvocationPostulant.InvestigationConvocationId);

            if (!evaluator)
            {
                return RedirectToPage("/Postulant/Index");
            }


            Input = new InvestigationConvocationPostulantViewModel
            {
                PercentA = investigationConvocationPostulant.InvestigationTypeId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight
                            + (investigationConvocationPostulant.ExternalEntityId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight)
                            + (investigationConvocationPostulant.Budget == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.BudgetWeight)
                            + (investigationConvocationPostulant.InvestigationPatternId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight)
                            + (investigationConvocationPostulant.InvestigationAreaId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.AreaWeight)
                            + (investigationConvocationPostulant.FacultyId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.FacultyWeight)
                            + (investigationConvocationPostulant.CareerId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.CareerWeight)
                            + (investigationConvocationPostulant.ResearchCenterId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight)
                            + (investigationConvocationPostulant.FinancingInvestigationId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight)
                            + (investigationConvocationPostulant.PostulantExecutionPlaces.Count == 0 ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.ProjectTitle) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.GeneralGoal) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.ProblemFormulation) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.SpecificGoal) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.Justification) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.JustificationWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.TheoreticalFundament) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.ProblemRecord) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.Hypothesis) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.Variable) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.VariableWeight)
                            + (investigationConvocationPostulant.MethodologyTypeId == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.MethodologyDescription) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.Population) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.PopulationWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.InformationCollectionTechnique) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.AnalysisTechnique) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight)

                            + (string.IsNullOrEmpty(investigationConvocationPostulant.EthicalConsiderations) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight)

                            + (string.IsNullOrEmpty(investigationConvocationPostulant.FieldWork) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight)

                            + (string.IsNullOrEmpty(investigationConvocationPostulant.ExpectedResults) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.BibliographicReferences) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight)

                            + (string.IsNullOrEmpty(investigationConvocationPostulant.ThesisDevelopment) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.PublishedArticle) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.BroadcastArticle) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight)
                            + (investigationConvocationPostulant.PostulantTeamMemberUsers.Count == 0 ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight)
                            + (investigationConvocationPostulant.PostulantExternalMembers.Count == 0 ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.ProcessDevelopment) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight)
                            + (investigationConvocationPostulant.ProjectDuration == null ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight)
                            + (string.IsNullOrEmpty(investigationConvocationPostulant.MainLocation) ? 0 : investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationWeight)
                            ,
                InvestigationConvocationRequirementId = investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.Id,
                InvestigationConvocationPostulantId = investigationConvocationPostulant.Id,
                InvestigationTypeText = investigationConvocationPostulant.InvestigationTypeId == null ? "" : investigationConvocationPostulant.InvestigationType.Name,
                ExternalEntityText = investigationConvocationPostulant.ExternalEntityId == null ? "" : investigationConvocationPostulant.ExternalEntity.Name,
                Budget = investigationConvocationPostulant.Budget.HasValue ? investigationConvocationPostulant.Budget.Value : 0,
                InvestigationPatternText = investigationConvocationPostulant.InvestigationPatternId == null ? "" : investigationConvocationPostulant.InvestigationPattern.Name,
                InvestigationAreaText = investigationConvocationPostulant.InvestigationAreaId == null ? "" : investigationConvocationPostulant.InvestigationArea.Name,
                FacultyText = investigationConvocationPostulant.FacultyId == null ? "" : investigationConvocationPostulant.FacultyText,
                CareerText = investigationConvocationPostulant.CareerId == null ? "" : investigationConvocationPostulant.CareerText,
                ResearchCenterText = investigationConvocationPostulant.ResearchCenterId == null ? "" : investigationConvocationPostulant.ResearchCenter.Name,
                FinancingInvestigationText = investigationConvocationPostulant.FinancingInvestigationId == null ? "" : investigationConvocationPostulant.FinancingInvestigation.Name,
                MainLocationHidden = investigationConvocationPostulant.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationHidden,
                MainLocation = investigationConvocationPostulant.MainLocation,
                ProjectTitle = investigationConvocationPostulant.ProjectTitle,
                GeneralGoal = investigationConvocationPostulant.GeneralGoal,
                ProblemFormulation = investigationConvocationPostulant.ProblemFormulation,
                SpecificGoal = investigationConvocationPostulant.SpecificGoal,
                Justification = investigationConvocationPostulant.Justification,
                TheoreticalFundament = investigationConvocationPostulant.TheoreticalFundament,
                ProblemRecord = investigationConvocationPostulant.ProblemRecord,
                Hypothesis = investigationConvocationPostulant.Hypothesis,
                Variable = investigationConvocationPostulant.Variable,
                MethodologyTypeText = investigationConvocationPostulant.MethodologyTypeId == null ? "" : investigationConvocationPostulant.MethodologyType.Name,
                MethodologyDescription = investigationConvocationPostulant.MethodologyDescription,
                Population = investigationConvocationPostulant.Population,
                InformationCollectionTechnique = investigationConvocationPostulant.InformationCollectionTechnique,
                AnalysisTechnique = investigationConvocationPostulant.AnalysisTechnique,
                //
                EthicalConsiderations = investigationConvocationPostulant.EthicalConsiderations,
                //
                FieldWork = investigationConvocationPostulant.FieldWork,

                //
                ExpectedResults = investigationConvocationPostulant.ExpectedResults,
                BibliographicReferences = investigationConvocationPostulant.BibliographicReferences,
                //
                ThesisDevelopment = investigationConvocationPostulant.ThesisDevelopment,
                PublishedArticle = investigationConvocationPostulant.PublishedArticle,
                BroadcastArticle = investigationConvocationPostulant.BroadcastArticle,
                ProcessDevelopment = investigationConvocationPostulant.ProcessDevelopment,
                UserId = investigationConvocationPostulant.UserId,
                ConvocationMinScore = investigationConvocationPostulant.InvestigationConvocation.MinScore,
                IsRated = investigationConvocationPostulant.PostulantRubricQualifications.Count() > 0,
                RateScore = investigationConvocationPostulant.PostulantRubricQualifications.Sum(x => x.Value),
                ProjectDuration = investigationConvocationPostulant.ProjectDuration,
                ProjectState = investigationConvocationPostulant.ProjectState,
                ProjectStateText = TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES.ContainsKey(investigationConvocationPostulant.ProjectState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES[investigationConvocationPostulant.ProjectState] : "",
                ProgressState = investigationConvocationPostulant.ProgressState,
                ProgressStateText = TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES.ContainsKey(investigationConvocationPostulant.ProgressState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES[investigationConvocationPostulant.ProgressState] : "",
                ReviewState = investigationConvocationPostulant.ReviewState,
                ReviewStateText = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(investigationConvocationPostulant.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[investigationConvocationPostulant.ReviewState] : "",
            };

            var answerByUsers = _context.InvestigationAnswerByUsers
                .Where(x => x.UserId == investigationConvocationPostulant.UserId && x.InvestigationQuestion.InvestigationConvocationRequirementId == Input.InvestigationConvocationRequirementId);

            var questions = await _context.InvestigationQuestions
                .Where(x => x.InvestigationConvocationRequirementId == Input.InvestigationConvocationRequirementId)
                .Select(x => new InvestigationQuestionsViewModel
                {
                    Id = x.Id,
                    QuestionDescription = x.Description,
                    IsRequired = x.IsRequired,
                    Type = x.Type,
                    AnswerDescription = answerByUsers.Where(au => au.InvestigationQuestionId == x.Id).Select(au => au.AnswerDescription).FirstOrDefault(),
                    UniqueAnswer = answerByUsers.Where(au => au.InvestigationQuestionId == x.Id).Select(au => au.InvestigationAnswerId).FirstOrDefault(),
                    InvestigationAnswers = x.InvestigationAnswers
                        .Select(y => new InvestigationAnswersViewModel
                        {
                            Id = y.Id,
                            Description = y.Description,
                            Selected = answerByUsers
                                .Any(au => au.InvestigationQuestionId == y.InvestigationQuestionId && au.InvestigationAnswerId == y.Id)
                        })
                        .ToList()
                })
                .ToListAsync();

            Input.InvestigationQuestions = questions;

            var postulantExecutionPlaces = await _context.PostulantExecutionPlaces
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .Select(x => new PostulantExecutionPlaceViewModel
                {
                    DepartmentText = x.DepartmentText,
                    ProvinceText = x.ProvinceText,
                    DistrictText = x.DistrictText
                })
                .ToListAsync();

            Input.PostulantExecutionPlaces = postulantExecutionPlaces;

            var postulantTeamMembers = await _context.PostulantTeamMemberUsers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .Select(x => new PostulantTeamMemberUserViewModel
                {
                    UserMemberFullName = x.User.FullName,
                    TeamMemberRole = x.TeamMemberRole.Name,
                    CvFilePath = x.CvFilePath,
                    Objectives = x.Objectives,
                }).ToListAsync();

            Input.PostulantTeamMembers = postulantTeamMembers;

            var postulantExternalMembers = await _context.PostulantExternalMembers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .Select(x => new PostulantExternalMemberViewModel
                {
                    FullName = $"{x.PaternalSurname} {x.MaternalSurname} {x.Name}",
                    Dni = x.Dni,
                    CvFilePath = x.CvFilePath,
                    Profession = x.Profession
                }).ToListAsync();

            Input.PostulantExternalMembers = postulantExternalMembers;

            return Page();
        }
    }
}
