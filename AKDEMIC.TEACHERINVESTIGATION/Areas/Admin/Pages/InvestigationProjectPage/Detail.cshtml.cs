using System;
using System.Collections.Generic;
using System.IO;
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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectViewModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationProjectPage
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

        public async Task<IActionResult> OnGetTaskDatatableAsync(Guid investigationProjectId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<DOMAIN.Entities.TeacherInvestigation.InvestigationProjectTask, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Description;
                    break;
                case "1":
                    orderByPredicate = (x) => x.User.FullName;
                    break;
                case "2":
                    orderByPredicate = (x) => x.CreatedAt;
                    break;
            }

            var query = _context.InvestigationProjectTasks
                .Where(x => x.InvestigationProjectId == investigationProjectId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    TaskName = x.Description,
                    x.User.FullName
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
                case "3":
                    orderByPredicate = (x) => x.InvestigationProjectTask.Description;
                    break;
                case "4":
                    orderByPredicate = (x) => x.ExpenseCode;
                    break;
                case "5":
                    orderByPredicate = (x) => x.ProductType;
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
                    x.ExpenseCode,
                    x.ProductType,
                    TaskDescription = x.InvestigationProjectTask.Description,
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

        public async Task<IActionResult> OnGetPostulantPdfReportAsync(Guid investigationProjectId)
        {
            var project = await _context.InvestigationProjects
                .Where(x => x.Id == investigationProjectId)
                .FirstOrDefaultAsync();

            if (project == null)
                return new BadRequestResult();

            var postulant = await _context.InvestigationConvocationPostulants
                .Where(x => x.Id == project.InvestigationConvocationPostulantId)
                .Select(x => new 
                {
                    ConvocationName = x.InvestigationConvocation.Name,
                    PostulantFullName = x.User.FullName,                  
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
                    InvestigationPattern = x.InvestigationPatternId == null ? "": x.InvestigationPattern.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight,
                    x.InvestigationAreaId,
                    InvestigationArea = x.InvestigationAreaId == null ? "": x.InvestigationArea.Name,
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
                    FinancingInvestigation = x.FinancingInvestigationId == null ? "": x.FinancingInvestigation.Name,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FinancingWeight,
                    //lugar de ejecucion
                    x.MainLocation,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.MainLocationWeight,
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
                    x.EthicalConsiderations,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight,
                    x.AnalysisTechnique,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight,
                    x.FieldWork,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight,
                    //5 Resultados esperados
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

                    //
                    x.ExpectedResults,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight,

                    x.BibliographicReferences,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesHidden,
                    x.InvestigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight,
                    //
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

            var modal = new PostulantReportPdfViewModel
            {
                PostulanFullName = postulant.PostulantFullName,
                ConvocationName = postulant.ConvocationName,

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

                FieldWork = postulant.FieldWork,
                FieldWorkHidden = postulant.FieldWorkHidden,

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
                .Where(x => x.UserId == postulant.UserId && x.InvestigationQuestion.InvestigationConvocationRequirementId == modal.InvestigationConvocationRequirementId);

            var questions = await _context.InvestigationQuestions
                .Where(x => x.InvestigationConvocationRequirementId == modal.InvestigationConvocationRequirementId)
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

            modal.InvestigationQuestions = questions;

            var postulantExecutionPlaces = await _context.PostulantExecutionPlaces
                .Where(x => x.InvestigationConvocationPostulantId == postulant.Id)
                .Select(x => new PostulantExecutionPlaceViewModel
                {
                    DepartmentText = x.DepartmentText,
                    ProvinceText = x.ProvinceText,
                    DistrictText = x.DistrictText
                })
                .ToListAsync();
            modal.ExecutionPlaceHidden = postulant.ExecutionPlaceHidden;
            modal.PostulantExecutionPlaces = postulantExecutionPlaces;

            var postulantTeamMembers = await _context.PostulantTeamMemberUsers
                .Where(x => x.InvestigationConvocationPostulantId == postulant.Id)
                .Select(x => new PostulantTeamMemberUserViewModel
                {
                    UserMemberFullName = x.User.FullName,
                    TeamMemberRole = x.TeamMemberRole.Name,
                    CvFilePath = x.CvFilePath,
                    Objectives = x.Objectives,
                }).ToListAsync();
            modal.TeamMemberUserHidden = postulant.TeamMemberUserHidden;
            modal.PostulantTeamMembers = postulantTeamMembers;

            var postulantExternalMembers = await _context.PostulantExternalMembers
                .Where(x => x.InvestigationConvocationPostulantId == postulant.Id)
                .Select(x => new PostulantExternalMemberViewModel
                {
                    FullName = $"{x.PaternalSurname} {x.MaternalSurname} {x.Name}",
                    Dni = x.Dni,
                    CvFilePath = x.CvFilePath,
                    Profession = x.Profession
                }).ToListAsync();

            modal.ExternalMemberHidden = postulant.ExternalMemberHidden;
            modal.PostulantExternalMembers = postulantExternalMembers;



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
            #endregion

            modal.ResearchLineCategoryRequirments = researchLineCategoryRequirmentViewModel;

            modal.ImageLogoPath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images\themes\{GeneralConstants.GetTheme()}\logo-report.png");

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Pages/InvestigationProjectPage/Partials/_PostulantReportPdfcshtml.cshtml", modal);

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                        FontName = "Arial",
                        FontSize = 9,
                        Line = false,
                        Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                        Center = "",
                        Right = "Pag: [page]/[toPage]"
                }
            };

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 10, Left = 10, Right = 10 }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";

            return File(fileByte, "application/pdf", "reporte_postulacion.pdf");

        }

    }
}
