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
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPostulantPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class DetailsModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;

        public DetailsModel(
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

        

        public async Task<IActionResult> OnPostSolveObservationAsync(Guid postulantObservationId)
        {
            var observation = await _context.PostulantObservations
                .Where(x => x.Id == postulantObservationId)
                .FirstOrDefaultAsync();

            if (observation == null)
                return new BadRequestObjectResult("Sucedio un error");

            observation.State = TeacherInvestigationConstants.PostulantObservation.State.PENDINGREVIEW;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetAnnexedFileDatatable(Guid investigationConvocationPostulantId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PostulantAnnexFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PostulantAnnexFiles
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.DocumentPath,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
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

        public async Task<IActionResult> OnGetObservationsDatatableAsync(Guid investigationConvocationPostulantId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.PostulantObservation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "1":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "3":
                    orderByPredicate = (x) => x.State;
                    break;
            }
                   
            var query = _context.PostulantObservations
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.Description,
                    x.State,
                    StateText = TeacherInvestigationConstants.PostulantObservation.State.VALUES.ContainsKey(x.State) ?
                        TeacherInvestigationConstants.PostulantObservation.State.VALUES[x.State] : ""
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

        public async Task<IActionResult> OnGet(Guid investigationConvocationPostulantId)
        {
            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x => new
                {
                    InvestigationConvocationRequirementId = x.InvestigationConvocation.InvestigationConvocationRequirement.Id,
                    x.InvestigationConvocationId,
                    x.Id,
                    x.UserId,
                    x.ProgressState,
                    x.ReviewState,
                    x.ProjectState,
                    //1 Datos generales
                    x.InvestigationTypeId,
                    InvestigationType = x.InvestigationTypeId == null ? "" : x.InvestigationType.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight,
                    x.ExternalEntityId,
                    ExternalEntity = x.ExternalEntityId == null ? "" : x.ExternalEntity.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight,
                    Budget = x.Budget,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BudgetWeight,
                    x.InvestigationPatternId,
                    InvestigationPattern = x.InvestigationPatternId == null ? "" : x.InvestigationPattern.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight,
                    x.InvestigationAreaId,
                    InvestigationArea = x.InvestigationAreaId == null ? "" : x.InvestigationArea.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AreaHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AreaWeight,
                    Faculty = x.FacultyId == null ? "" : x.FacultyText,
                    x.FacultyId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FacultyWeight,
                    Career = x.CareerId == null ? "" : x.CareerText,
                    x.CareerId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.CareerHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.CareerWeight,
                    ResearchCenter = x.ResearchCenterId == null ? "" : x.ResearchCenter.Name,
                    x.ResearchCenterId,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight,
                    x.FinancingInvestigationId,
                    FinancingInvestigation = x.FinancingInvestigationId == null ? "" : x.FinancingInvestigation.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight,
                    //lugar de ejecucion
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight,
                    ExecutionPlaces = x.PostulantExecutionPlaces
                        .Select(y => new
                        {
                            y.Id,
                            y.DepartmentId,
                            y.DepartmentText,
                            y.ProvinceId,
                            y.ProvinceText,
                            y.DistrictId,
                            y.DistrictText
                        })
                        .ToList(),
                    ResearchLineCategoryRequirements = x.InvestigationConvocation.InvestigationConvocationRequirement.ResearchLineCategoryRequirements
                            .Select(y => new
                            {
                                y.Id,
                                y.ResearchLineCategoryId,
                                ResearchLineCategoryName = y.ResearchLineCategory.Name,
                                y.Hidden,
                                y.Weight,
                                Selected = x.PostulantResearchLines
                                    .Where(z => z.ResearchLine.ResearchLineCategoryId == y.ResearchLineCategoryId)
                                    .Any()
                            })
                            .ToList(),

                    //localización principal del proyecto
                    x.MainLocation,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationWeight,
                    //2 Descripción del Problema
                    x.ProjectTitle,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight,
                    x.ProblemDescription,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight,
                    x.GeneralGoal,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight,
                    x.ProblemFormulation,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight,
                    x.SpecificGoal,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight,
                    x.Justification,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.JustificationWeight,
                    //3 Marco de Referencia
                    x.TheoreticalFundament,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight,
                    x.ProblemRecord,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight,
                    x.Hypothesis,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.HypothesisWeight,
                    x.Variable,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.VariableHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.VariableWeight,
                    //4 Metodologia
                    x.MethodologyTypeId,
                    MethodologyType = x.MethodologyTypeId == null ? "" : x.MethodologyType.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight,
                    x.MethodologyDescription,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight,
                    x.Population,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PopulationWeight,
                    x.InformationCollectionTechnique,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight,
                    x.AnalysisTechnique,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight,
                    //
                    x.EthicalConsiderations,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight,
                    //
                    x.FieldWork,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight,
                    //5 Resultados esperados
                    //
                    x.ExpectedResults,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight,
                    x.BibliographicReferences,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight,
                    //
                    x.ThesisDevelopment,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight,
                    x.PublishedArticle,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight,
                    x.BroadcastArticle,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight,
                    x.ProcessDevelopment,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight,
                    //6.Equipo de trabajo,

                    x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight,
                    PostulantTeamMemberUsers = x.PostulantTeamMemberUsers
                        .Select(y => new 
                        {
                            y.Id,
                            y.User.FullName,
                            RoleName = y.TeamMemberRole.Name
                        })
                        .ToList(),
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight,
                    ExternalMembers = x.PostulantExternalMembers
                        .Select(y => new 
                        {
                            y.Id,
                            FullName = $"{y.PaternalSurname} {y.MaternalSurname} {y.Name}",
                            y.Dni,
                            y.Profession
                        })
                        .ToList(),
                    //7.Anexos Adjuntos
                    x.ProjectDuration,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight,
                    PostulationAttachmentFiles = x.PostulantAnnexFiles
                        .Select(y => new 
                        { 
                            y.Id,
                            y.Name
                        })
                        .ToList(),
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight,
                    //8.Preguntas Adicionales
                    AdditionalQuestions = _context.InvestigationAnswerByUsers
                        .Where(y => y.InvestigationQuestion.InvestigationConvocationRequirement.InvestigationConvocation.Id == x.InvestigationConvocationId && y.UserId == x.UserId)
                        .Select(y => new 
                        {
                            y.Id,
                            y.UserId,
                            y.InvestigationAnswerId,
                            y.AnswerDescription,
                            y.InvestigationQuestionId,
                            y.InvestigationQuestion.InvestigationConvocationRequirementId
                        })
                        .ToList(),
                    x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.QuestionsWeight,
                })
                .FirstOrDefaultAsync();

            var result = new AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels.InscriptionProgressDataViewModel
            {
                //1
                GeneralInformationPercentage = (postulant.InvestigationTypeHidden ? 0 : (postulant.InvestigationTypeId == null ? 0 : postulant.InvestigationTypeWeight))
                    + (postulant.ExternalEntityHidden ? 0 : (postulant.ExternalEntityId == null ? 0 : postulant.ExternalEntityWeight))
                    + (postulant.BudgetHidden ? 0 : (postulant.Budget == null ? 0 : postulant.BudgetWeight))
                    + (postulant.InvestigationPatternHidden ? 0 : (postulant.InvestigationPatternId == null ? 0 : postulant.InvestigationPatternWeight))
                    + (postulant.AreaHidden ? 0 : (postulant.InvestigationAreaId == null ? 0 : postulant.AreaWeight))
                    + (postulant.FacultyHidden ? 0 : (postulant.FacultyId == null ? 0 : postulant.FacultyWeight))
                    + (postulant.CareerHidden ? 0 : (postulant.CareerId == null ? 0 : postulant.CareerWeight))
                    + (postulant.ResearchCenterHidden ? 0 : (postulant.ResearchCenterId == null ? 0 : postulant.ResearchCenterWeight))
                    + (postulant.FinancingHidden ? 0 : (postulant.FinancingInvestigationId == null ? 0 : postulant.FinancingWeight))
                    + (postulant.ExecutionPlaceHidden ? 0 : (postulant.ExecutionPlaces.Count == 0 ? 0 : postulant.ExecutionPlaceWeight))
                    + (postulant.MainLocationHidden ? 0 : (postulant.MainLocation == null ? 0 : postulant.MainLocationWeight))
                    + postulant.ResearchLineCategoryRequirements.Where(x => !x.Hidden && x.Selected).Sum(x => x.Weight),
                //2
                ProblemDescriptionPercentage = (postulant.ProjectTitleHidden ? 0 : (postulant.ProjectTitle == null ? 0 : postulant.ProjectTitleWeight))
                    + (postulant.ProblemDescriptionHidden ? 0 : (postulant.ProblemDescription == null ? 0 : postulant.ProblemDescriptionWeight))
                    + (postulant.GeneralGoalHidden ? 0 :(postulant.GeneralGoal == null ? 0: postulant.GeneralGoalWeight))
                    + (postulant.ProblemFormulationHidden ? 0 :(postulant.ProblemFormulation == null ? 0: postulant.ProblemFormulationWeight))
                    + (postulant.SpecificGoalHidden ? 0 :(postulant.SpecificGoal == null ? 0: postulant.SpecificGoalWeight))
                    + (postulant.JustificationHidden ? 0 :(postulant.Justification == null ? 0 : postulant.JustificationWeight)),
                //3
                MarkReferencePercentage = (postulant.TheoreticalFundamentHidden ? 0:(postulant.TheoreticalFundament == null ? 0 : postulant.TheoreticalFundamentWeight))
                    + (postulant.ProblemRecordHidden ? 0 :(postulant.ProblemRecord == null ? 0: postulant.ProblemRecordWeight ))
                    + (postulant.HypothesisHidden ?0:(postulant.Hypothesis == null ?0: postulant.HypothesisWeight))
                    + (postulant.VariableHidden ?0:(postulant.Variable == null ?0: postulant.VariableWeight)),
                //4
                MethodologyPercentage = (postulant.MethodologyTypeHidden ? 0: (postulant.MethodologyTypeId == null ?0: postulant.MethodologyTypeWeight))
                    + (postulant.MethodologyDescriptionHidden ? 0 : (postulant.MethodologyDescription == null ? 0 : postulant.MethodologyDescriptionWeight))
                    + (postulant.PopulationHidden ? 0: (postulant.Population == null ? 0: postulant.PopulationWeight))
                    + (postulant.InformationCollectionTechniqueHidden ? 0:(postulant.InformationCollectionTechnique == null ? 0 : postulant.InformationCollectionTechniqueWeight))
                    + (postulant.AnalysisTechniqueHidden ? 0 : (postulant.AnalysisTechnique == null ? 0: postulant.AnalysisTechniqueWeight))
                    + (postulant.EthicalConsiderationsHidden ? 0 : (postulant.EthicalConsiderations == null ? 0 : postulant.EthicalConsiderationsWeight))
                    + (postulant.FieldWorkHidden ? 0 : (postulant.FieldWork == null ? 0: postulant.FieldWorkWeight)),
                //5
                ExpectedResultPercentage = (postulant.ExpectedResultsHidden ? 0 : (postulant.ExpectedResults == null ? 0 : postulant.ExpectedResultsWeight))
                    + (postulant.ThesisDevelopmentHidden ? 0:(postulant.ThesisDevelopment == null ?0 : postulant.ThesisDevelopmentWeight))
                    + (postulant.PublishedArticleHidden ? 0:(postulant.PublishedArticle == null ? 0 : postulant.PublishedArticleWeight))
                    + (postulant.BroadcastArticleHidden ? 0:(postulant.BroadcastArticle == null ? 0 : postulant.BroadcastArticleWeight))
                    + (postulant.ProcessDevelopmentHidden ? 0:(postulant.ProcessDevelopment == null ?0 : postulant.ProcessDevelopmentWeight))
                    + (postulant.BibliographicReferencesHidden ? 0 : (postulant.BibliographicReferences == null ? 0 : postulant.BibliographicReferencesWeight))
                    ,
                //6
                TeamMemberPercentage = postulant.TeamMemberUserHidden ? 0 : (postulant.PostulantTeamMemberUsers.Count == 0 ? 0 : postulant.TeamMemberUserWeight)
                    + (postulant.ExternalMemberHidden ? 0 : (postulant.ExternalMembers.Count == 0 ? 0 : postulant.ExternalMemberWeight)),
                //7
                AnnexFilesPercentage = (postulant.ProjectDurationHidden ? 0 : (postulant.ProjectDuration == null ? 0 : postulant.ProjectDurationWeight)) 
                    + ( postulant.PostulationAttachmentFilesHidden ? 0 : (postulant.PostulationAttachmentFiles.Count == 0 ? 0 : postulant.PostulationAttachmentFilesWeight)),
                //8
                AdditionalQuestionsPercentage = postulant.QuestionsHidden ? 0 : ( postulant.AdditionalQuestions.Count == 0 ? 0: postulant.QuestionsWeight)
            };

            result.ProgressBarPercentage = result.GeneralInformationPercentage +
                result.ProblemDescriptionPercentage +
                result.MarkReferencePercentage +
                result.MethodologyPercentage +
                result.ExpectedResultPercentage +
                result.TeamMemberPercentage +
                result.AnnexFilesPercentage +
                result.AdditionalQuestionsPercentage;

            Input = new InvestigationConvocationPostulantViewModel
            {
                PercentA = result.ProgressBarPercentage,

                InvestigationConvocationRequirementId = postulant.InvestigationConvocationRequirementId,
                InvestigationConvocationPostulantId = postulant.Id,
                UserId = postulant.UserId,

                InvestigationTypeText = postulant.InvestigationType,
                InvestigationTypeHidden = postulant.InvestigationTypeHidden,

                ExternalEntityText = postulant.ExternalEntity,
                ExternalEntityHidden = postulant.ExternalEntityHidden,

                Budget = postulant.Budget,
                BudgetHidden = postulant.BudgetHidden,

                InvestigationPatternText = postulant.InvestigationPattern,
                InvestigationPatternHidden = postulant.InvestigationPatternHidden,

                InvestigationAreaText = postulant.InvestigationArea,
                InvestigationAreaHidden = postulant.AreaHidden,

                FacultyText = postulant.Faculty,
                FacultyHidden = postulant.FacultyHidden,

                CareerText = postulant.Career,
                CareerHidden = postulant.CareerHidden,

                ResearchCenterText = postulant.ResearchCenter,
                ResearchCenterHidden = postulant.ResearchCenterHidden,

                FinancingInvestigationText = postulant.FinancingInvestigation,
                FinancingHidden = postulant.FinancingHidden,

                ProjectTitle = postulant.ProjectTitle,
                ProjectTitleHidden = postulant.ProjectTitleHidden,

                ProblemDescription = postulant.ProblemDescription,
                ProblemDescriptionHidden = postulant.ProblemDescriptionHidden,

                GeneralGoal = postulant.GeneralGoal,
                GeneralGoalHidden = postulant.GeneralGoalHidden,

                ProblemFormulation = postulant.ProblemFormulation,
                ProblemFormulationHidden = postulant.ProblemRecordHidden,

                SpecificGoal = postulant.SpecificGoal,
                SpecificGoalHidden = postulant.SpecificGoalHidden,

                Justification = postulant.Justification,
                JustificationHidden = postulant.JustificationHidden,

                TheoreticalFundament = postulant.TheoreticalFundament,
                TheoreticalFundamentHidden = postulant.TheoreticalFundamentHidden,

                ProblemRecord = postulant.ProblemRecord,
                ProblemRecordHidden = postulant.ProblemRecordHidden,

                Hypothesis = postulant.Hypothesis,
                HypothesisHidden = postulant.HypothesisHidden,

                Variable = postulant.Variable,
                VariableHidden = postulant.VariableHidden,

                MethodologyTypeText = postulant.MethodologyType,
                MethodologyTypeHidden = postulant.MethodologyTypeHidden,

                MethodologyDescription = postulant.MethodologyDescription,
                MethodologyDescriptionHidden = postulant.MethodologyDescriptionHidden,

                Population = postulant.Population,
                PopulationHidden = postulant.PopulationHidden,

                InformationCollectionTechnique = postulant.InformationCollectionTechnique,
                InformationCollectionTechniqueHidden = postulant.InformationCollectionTechniqueHidden,

                AnalysisTechnique = postulant.AnalysisTechnique,
                AnalysisTechniqueHidden = postulant.AnalysisTechniqueHidden,

                EthicalConsiderations = postulant.EthicalConsiderations,
                EthicalConsiderationsHidden = postulant.EthicalConsiderationsHidden,

                FieldWork = postulant.FieldWork,
                FieldWorkHidden = postulant.FieldWorkHidden,

                //
                ExpectedResults = postulant.ExpectedResults,
                ExpectedResultsHidden = postulant.ExpectedResultsHidden,

                BibliographicReferences = postulant.BibliographicReferences,
                BibliographicReferencesHidden = postulant.BibliographicReferencesHidden,
                //


                ThesisDevelopment = postulant.ThesisDevelopment,
                ThesisDevelopmentHidden = postulant.ThesisDevelopmentHidden,

                PublishedArticle = postulant.PublishedArticle,
                PublishedArticleHidden = postulant.PublishedArticleHidden,

                BroadcastArticle = postulant.BroadcastArticle,
                BroadcastArticleHidden = postulant.BroadcastArticleHidden,

                ProcessDevelopment = postulant.ProcessDevelopment,
                ProcessDevelopmentHidden = postulant.ProcessDevelopmentHidden,

                ProjectDuration = postulant.ProjectDuration,
                ProjectDurationHidden = postulant.ProjectDurationHidden,

                PostulationAttachmentFilesHidden = postulant.PostulationAttachmentFilesHidden,

                ProjectState = postulant.ProjectState,
                ProgressState = postulant.ProgressState,
                ReviewState = postulant.ReviewState,
                ProjectStateText = TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES.ContainsKey(postulant.ProjectState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProjectState.VALUES[postulant.ProjectState] : "",
                ProgressStateText = TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES.ContainsKey(postulant.ProgressState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ProgressState.VALUES[postulant.ProgressState] : "",
                ReviewStateText = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(postulant.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[postulant.ReviewState] : "",
            };

                

            var answerByUsers = _context.InvestigationAnswerByUsers
                .Where(x => x.UserId == postulant.UserId && x.InvestigationQuestion.InvestigationConvocationRequirementId == Input.InvestigationConvocationRequirementId);

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
            Input.ExecutionPlaceHidden = postulant.ExecutionPlaceHidden;
            Input.PostulantExecutionPlaces = postulantExecutionPlaces;

            Input.MainLocation = postulant.MainLocation;
            Input.MainLocationHidden= postulant.MainLocationHidden;

            var postulantTeamMembers = await _context.PostulantTeamMemberUsers
                .Where(x => x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .Select(x => new PostulantTeamMemberUserViewModel
                {
                    UserMemberFullName = x.User.FullName,
                    TeamMemberRole = x.TeamMemberRole.Name,
                    CvFilePath = x.CvFilePath,
                    Objectives = x.Objectives,
                }).ToListAsync();
            Input.TeamMemberUserHidden = postulant.TeamMemberUserHidden;
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
            Input.ExternalMemberHidden = postulant.ExternalMemberHidden;
            Input.PostulantExternalMembers = postulantExternalMembers;

            #region researchLineCategoryRequirment
            var researchLineCategoryRequirmentViewModel = new List<ResearchLineCategoryRequirmentViewModel>();

            var researchLineCategoriesRequirement = await _context.ResearchLineCategoryRequirements
                .Where(x => x.InvestigationConvocationRequirement.InvestigationConvocation.Id == postulant.InvestigationConvocationId && !x.Hidden)
                .Select(x => new
                {
                    InvestigationConvocationId = x.InvestigationConvocationRequirement.InvestigationConvocation.Id,
                    InvestigationConvocationRequirementId = x.InvestigationConvocationRequirementId,
                    x.Id,
                    x.Hidden,
                    x.Weight,
                    x.ResearchLineCategoryId,
                    ResearchLineCategoryName = x.ResearchLineCategory.Name
                })
                .ToListAsync();

            var postulantResearchLine = await _context.PostulantResearchLines
                .Where(x => x.InvestigationConvocationPostulantId == postulant.Id)
                .Select(x => new
                {
                    x.InvestigationConvocationPostulant.InvestigationConvocationId,
                    x.ResearchLine.ResearchLineCategoryId,
                    x.ResearchLineId,
                    ResearchLineName = x.ResearchLine.Name,
                    x.Id
                })
                .ToListAsync();

            //En base a los requerimientos de categorias agregados
            for (int i = 0; i < researchLineCategoriesRequirement.Count; i++)
            {
                var postulantDataSelected = postulantResearchLine
                    .Where(x => x.InvestigationConvocationId == researchLineCategoriesRequirement[i].InvestigationConvocationId &&
                                x.ResearchLineCategoryId == researchLineCategoriesRequirement[i].ResearchLineCategoryId)
                    .FirstOrDefault();

                var researchLinePostulant = new ResearchLineCategoryRequirmentViewModel
                {
                    Hidden = researchLineCategoriesRequirement[i].Hidden,
                    ResearchLineCategoryName = researchLineCategoriesRequirement[i].ResearchLineCategoryName
                };

                if (postulantDataSelected != null)
                {
                    researchLinePostulant.ResearchLineSelected = postulantDataSelected.ResearchLineName;
                }

                researchLineCategoryRequirmentViewModel.Add(researchLinePostulant);
            }

            Input.ResearchLineCategoryRequirments = researchLineCategoryRequirmentViewModel;

            #endregion

            return Page();     
        }

        public async Task<IActionResult> OnPostObservationCreateAsync(InvestigationConvocationObservationCreateModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Verifique los datos ingresados");

            //var observationAdd = await _context.PostulantObservations
            //   .Where(x => x.InvestigationConvocationPostulantId == investigationconvocationpostulantId)
            //   .FirstOrDefaultAsync();

            // if (observationAdd == null)
            //    return new BadRequestObjectResult("Sucedio un error");

            var postulantconvocation = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == Input.InvestigationConvocationPostulantId)
                .FirstOrDefaultAsync();

            if (postulantconvocation.ReviewState == TeacherInvestigationConstants.ConvocationPostulant.ReviewState.DECLINED ||
                postulantconvocation.ReviewState == TeacherInvestigationConstants.ConvocationPostulant.ReviewState.ADMITTED)
            {
                return new BadRequestObjectResult("Esta postulación ya fue revisada");
            }

            var observationAdd = new DOMAIN.Entities.TeacherInvestigation.PostulantObservation
            {
                Description = model.Description,
                State = TeacherInvestigationConstants.PostulantObservation.State.PENDINGCORRECTION,
                InvestigationConvocationPostulantId = model.investigationconvocationpostulantId,
            };

            

            postulantconvocation.ReviewState = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.OBSERVED;

            await _context.PostulantObservations.AddAsync(observationAdd);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostAbsolvedObservationAsync (Guid postulantObservationId)
        {
            var observation = await _context.PostulantObservations
                .Include(x=> x.InvestigationConvocationPostulant)
                .Where(x => x.Id == postulantObservationId)
                .FirstOrDefaultAsync();


            if (observation == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (observation.State != TeacherInvestigationConstants.PostulantObservation.State.PENDINGREVIEW)
                return new BadRequestObjectResult("El estado de la revisión es distinto de Pendiente de Revisión ");

            var haspendingObservation = await _context.PostulantObservations
                .Where(x => x.InvestigationConvocationPostulantId == x.InvestigationConvocationPostulantId && x.Id != postulantObservationId )
                .AnyAsync(x => x.State == TeacherInvestigationConstants.PostulantObservation.State.PENDINGREVIEW || x.State == TeacherInvestigationConstants.PostulantObservation.State.PENDINGCORRECTION);

            if (haspendingObservation == false)
            {
                observation.InvestigationConvocationPostulant.ReviewState = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.PENDING;
            }
           
            observation.State = TeacherInvestigationConstants.PostulantObservation.State.CORRECTED;

            await _context.SaveChangesAsync();


            return new OkResult();

        }
        public async Task<IActionResult> OnPostRejectObservationAsync(Guid postulantObservationId)
        {
            var observation = await _context.PostulantObservations
                .Where(x => x.Id == postulantObservationId)
                .FirstOrDefaultAsync();

            var postulantconvocation = await _context.InvestigationConvocationPostulants
               .Where(x => x.Id == Input.InvestigationConvocationPostulantId)
               .FirstOrDefaultAsync();

            if (observation == null)
                return new BadRequestObjectResult("Sucedio un error");

            

            if (observation.State == TeacherInvestigationConstants.PostulantObservation.State.PENDINGREVIEW)
            {
                observation.State = TeacherInvestigationConstants.PostulantObservation.State.PENDINGCORRECTION;
                await _context.SaveChangesAsync();
            }

            return new OkResult();

        }
        public async Task<IActionResult> OnPostAcceptAsync(Guid investigationConvocationPostulantId)
        {

            var convocationpostulant = await _context.InvestigationConvocationPostulants
               .Where(x => x.Id == investigationConvocationPostulantId)
               .FirstOrDefaultAsync();


            if (convocationpostulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            var haspendingObservation = await _context.PostulantObservations
                .Where(x => x.InvestigationConvocationPostulantId == convocationpostulant.Id)
                .AnyAsync(x => x.State == TeacherInvestigationConstants.PostulantObservation.State.PENDINGREVIEW || x.State == TeacherInvestigationConstants.PostulantObservation.State.PENDINGCORRECTION);

            if (convocationpostulant.ReviewState != TeacherInvestigationConstants.ConvocationPostulant.ReviewState.PENDING)
            {
                return new BadRequestObjectResult("Esta postulación esta en proceso");
            }

            if (haspendingObservation == true)
            {
                return new BadRequestObjectResult("Existe una o más observaciones Pendientes");
            }

            convocationpostulant.ReviewState = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.ADMITTED;

            await _context.SaveChangesAsync();


            return new OkResult();

        }
        public async Task<IActionResult> OnPostRejectAsync(Guid investigationConvocationPostulantId)
        {
            var convocationpostulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == investigationConvocationPostulantId)
                .FirstOrDefaultAsync();


            if (convocationpostulant == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (convocationpostulant.ReviewState != TeacherInvestigationConstants.ConvocationPostulant.ReviewState.PENDING)
            {
                return new BadRequestObjectResult("Esta postulación esta en proceso");
            }

            convocationpostulant.ReviewState = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.DECLINED;

            await _context.SaveChangesAsync();


            return new OkResult();

        }
    }
}
