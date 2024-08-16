using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class InscriptionFormModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IAsyncRepository<InvestigationQuestion> _investigationQuestion;

        public InscriptionFormModel(
            IAsyncRepository<InvestigationQuestion> investigationQuestion,
            AkdemicContext context
        )
        {
            _context = context;
            _investigationQuestion = investigationQuestion;
        }
        [BindProperty]
        public ConvocationInscriptionFormViewModel Input { get; set; }

        #region Questions

        //Obtener vista parcial de preguntas
        public async Task<IActionResult> OnGetQuestionsAsync(Guid investigationConvocationRequirementId)
        {
            var questions = await _context.InvestigationQuestions
                .Where(x => x.InvestigationConvocationRequirementId == investigationConvocationRequirementId)
                .Select(x => new InvestigationQuestionViewModel
                {
                    Id = x.Id,
                    Description = x.Description,
                    IsRequired = x.IsRequired,
                    QuestionType = x.Type,
                    InvestigationConvocationRequirementId = x.InvestigationConvocationRequirementId,
                    InvestigationAnswers = x.InvestigationAnswers
                        .Select(y => new InvestigationAnswerViewModel
                        {
                            Id = y.Id,
                            Description = y.Description
                        })
                        .ToList()
                })
                .ToListAsync();

            return Partial("Partials/_Questions", questions);
        }


        /// <summary>
        /// Obtiene informaci?n de una pregunta, para cargar en el modal
        /// </summary>
        /// <param name="investigationQuestionId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetLoadQuestionAsync(Guid investigationQuestionId)
        {
            var question = await _context.InvestigationQuestions
                .Where(x => x.Id == investigationQuestionId)
                .Select(x => new InvestigationQuestionViewModel
                {
                    //Agregar campos de la pregunta
                    Id = x.Id,
                    Description = x.Description,
                    IsRequired = x.IsRequired,
                    QuestionType = x.Type,
                    InvestigationConvocationRequirementId = x.InvestigationConvocationRequirementId,
                    InvestigationAnswers = x.InvestigationAnswers
                        .Select(y => new InvestigationAnswerViewModel 
                        {
                            Id = y.Id,
                            Description = y.Description
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            return new OkObjectResult(question);
        }
        /// <summary>
        /// Agregar las preguntas
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostAddQuestionAsync(InvestigationQuestionCreateViewModel model)
        {

            if (!ModelState.IsValid) return BadRequest("Verifique los datos Ingresados");



            var question = new InvestigationQuestion
            {
                IsRequired = model.IsRequired,
                Description = model.Description,
                Type = model.QuestionType,
                InvestigationConvocationRequirementId = model.InvestigationConvocationRequirementId,
                InvestigationAnswers = new List<InvestigationAnswer>()
            };

            var answersQuantity = 0;

            if (model.Answers != null) answersQuantity = model.Answers.Count();

            if (model.QuestionType == CORE.Constants.Systems.TeacherInvestigationConstants.InscriptionForm.QuestionType.UNIQUE_SELECTION_QUESTION || model.QuestionType == CORE.Constants.Systems.TeacherInvestigationConstants.InscriptionForm.QuestionType.MULTIPLE_SELECTION_QUESTION)
            {
                if (answersQuantity < 2) return BadRequest("Agregue por lo menos dos respuestas");
                foreach (var answer in model.Answers)
                {
                    var newAnswer = new InvestigationAnswer
                    {
                        Description = answer.Description
                    };
                    question.InvestigationAnswers.Add(newAnswer);
                }
            }

            await _context.InvestigationQuestions.AddAsync(question);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        /// <summary>
        /// Editar la preguntas del punto 8.0 del formulario ,
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnPostEditQuestionAsync(InvestigationQuestionEditViewModel model)
        {

            if (!ModelState.IsValid) return BadRequest("Verifique los datos Ingresados");

            var question = await _context.InvestigationQuestions
                .Include(x=>x.InvestigationAnswers)
                .Where(x => x.Id == model.Id).FirstOrDefaultAsync();

            if(question == null) return BadRequest("Hay un error");

            question.IsRequired = model.IsRequired;
            question.Description = model.Description;
            question.Type = model.QuestionType;



           //BORRAR RESPUESTAS QUE EXISTEN
            if (question.InvestigationAnswers.Count() > 0)
            {
                _context.InvestigationAnswers.RemoveRange(question.InvestigationAnswers);
            } 
            var answersQuantity = 0;

            if (model.InvestigationAnswers != null) answersQuantity = model.InvestigationAnswers.Count();

            if (model.QuestionType == CORE.Constants.Systems.TeacherInvestigationConstants.InscriptionForm.QuestionType.UNIQUE_SELECTION_QUESTION || model.QuestionType == CORE.Constants.Systems.TeacherInvestigationConstants.InscriptionForm.QuestionType.MULTIPLE_SELECTION_QUESTION)
            {
                if (answersQuantity < 2) return BadRequest("Agregue por lo menos dos respuestas");
                foreach (var answer in model.InvestigationAnswers)
                {
                    var newAnswer = new InvestigationAnswer
                    {
                        Description = answer.Description
                    };
                    question.InvestigationAnswers.Add(newAnswer);
                }
            }

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        /// <summary>
        /// Eliminar preguntas del formulario.
        /// </summary>
        /// <param name="investigationQuestionId"></param>
        /// <returns></returns>
        //Eliminar pregunta
        public async Task<IActionResult> OnPostDeleteQuestionAsync(Guid investigationQuestionId)
        {
            var question = await _context.InvestigationQuestions
                .Include(x => x.InvestigationAnswers)
                .Where(x => x.Id == investigationQuestionId)
                .FirstOrDefaultAsync();

            if (question == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (question.InvestigationAnswers != null && question.InvestigationAnswers.Count > 0)
            {
                _context.InvestigationAnswers.RemoveRange(question.InvestigationAnswers);
            }

            _context.InvestigationQuestions.Remove(question);

            await _context.SaveChangesAsync();

            return new OkResult();
        }


        #endregion




        /// <summary>
        /// Obtener la data del formulario ya guardarda dentro del formulario ingresado en la convocatoria.
        /// </summary>
        /// <param name="investigationConvocationId"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGet(Guid investigationConvocationId)
        {
            var investigationConvocation = await _context.InvestigationConvocations
                .Include(x => x.InvestigationConvocationRequirement)
                    .ThenInclude(y => y.ResearchLineCategoryRequirements)
                .Where(x => x.Id == investigationConvocationId)
                .FirstOrDefaultAsync();

            if (investigationConvocation == null)
            {
                return RedirectToPage("/InvestigationConvocationPage/Index");
            }

            if (investigationConvocation.InvestigationConvocationRequirement == null)
            {
                investigationConvocation.InvestigationConvocationRequirement = new InvestigationConvocationRequirement();

                await _context.SaveChangesAsync();
            }
            var investigationQuestions = await _context.InvestigationQuestions
                .Where(x => x.InvestigationConvocationRequirementId == investigationConvocationId)
                .FirstOrDefaultAsync();

            int researchLineCategoryRequirementsSum = 0;

            if (investigationConvocation.InvestigationConvocationRequirement.ResearchLineCategoryRequirements.Count > 0)
                researchLineCategoryRequirementsSum = investigationConvocation.InvestigationConvocationRequirement.ResearchLineCategoryRequirements.Sum(x => x.Weight);

            Input = new ConvocationInscriptionFormViewModel
            {
                InvestigationConvocationId = investigationConvocation.Id,
                InvestigationConvocationRequirementId = investigationConvocation.InvestigationConvocationRequirement.Id,
                InvestigationTypeHidden = investigationConvocation.InvestigationConvocationRequirement.InvestigationTypeHidden,
                InvestigationTypeWeight = investigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight,
                ExternalEntityHidden = investigationConvocation.InvestigationConvocationRequirement.ExternalEntityHidden,
                ExternalEntityWeight = investigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight,
                BudgetHidden = investigationConvocation.InvestigationConvocationRequirement.BudgetHidden,
                BudgetWeight = investigationConvocation.InvestigationConvocationRequirement.BudgetWeight,
                InvestigationPatternHidden = investigationConvocation.InvestigationConvocationRequirement.InvestigationPatternHidden,
                InvestigationPatternWeight = investigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight,
                AreaHidden = investigationConvocation.InvestigationConvocationRequirement.AreaHidden,
                AreaWeight = investigationConvocation.InvestigationConvocationRequirement.AreaWeight,
                FacultyHidden = investigationConvocation.InvestigationConvocationRequirement.FacultyHidden,
                FacultyWeight = investigationConvocation.InvestigationConvocationRequirement.FacultyWeight,
                CareerHidden = investigationConvocation.InvestigationConvocationRequirement.CareerHidden,
                CareerWeight = investigationConvocation.InvestigationConvocationRequirement.CareerWeight,
                ResearchCenterHidden = investigationConvocation.InvestigationConvocationRequirement.ResearchCenterHidden,
                ResearchCenterWeight = investigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight,
                FinancingHidden = investigationConvocation.InvestigationConvocationRequirement.FinancingHidden,
                FinancingWeight = investigationConvocation.InvestigationConvocationRequirement.FinancingWeight,
                MainLocationHidden = investigationConvocation.InvestigationConvocationRequirement.MainLocationHidden,
                MainLocationWeight = investigationConvocation.InvestigationConvocationRequirement.MainLocationWeight,
                ExecutionPlaceHidden = investigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceHidden,
                ExecutionPlaceWeight = investigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight,
                ProjectTitleHidden = investigationConvocation.InvestigationConvocationRequirement.ProjectTitleHidden,
                ProjectTitleWeight = investigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight,
                ProblemDescriptionHidden = investigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionHidden,
                ProblemDescriptionWeight = investigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight,
                GeneralGoalHidden = investigationConvocation.InvestigationConvocationRequirement.GeneralGoalHidden,
                GeneralGoalWeight = investigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight,
                ProblemFormulationHidden = investigationConvocation.InvestigationConvocationRequirement.ProblemFormulationHidden,
                ProblemFormulationWeight = investigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight,
                SpecificGoalHidden = investigationConvocation.InvestigationConvocationRequirement.SpecificGoalHidden,
                SpecificGoalWeight = investigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight,
                JustificationHidden = investigationConvocation.InvestigationConvocationRequirement.JustificationHidden,
                JustificationWeight = investigationConvocation.InvestigationConvocationRequirement.JustificationWeight,
                TheoreticalFundamentHidden = investigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentHidden,
                TheoreticalFundamentWeight = investigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight,
                ProblemRecordHidden = investigationConvocation.InvestigationConvocationRequirement.ProblemRecordHidden,
                ProblemRecordWeight = investigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight,
                HypothesisHidden = investigationConvocation.InvestigationConvocationRequirement.HypothesisHidden,
                HypothesisWeight = investigationConvocation.InvestigationConvocationRequirement.HypothesisWeight,
                VariableHidden = investigationConvocation.InvestigationConvocationRequirement.VariableHidden,
                VariableWeight = investigationConvocation.InvestigationConvocationRequirement.VariableWeight,
                MethodologyTypeHidden = investigationConvocation.InvestigationConvocationRequirement.MethodologyTypeHidden,
                MethodologyTypeWeight = investigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight,
                MethodologyDescriptionHidden = investigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionHidden,
                MethodologyDescriptionWeight = investigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight,
                PopulationHidden = investigationConvocation.InvestigationConvocationRequirement.PopulationHidden,
                PopulationWeight = investigationConvocation.InvestigationConvocationRequirement.PopulationWeight,
                InformationCollectionTechniqueHidden = investigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueHidden,
                InformationCollectionTechniqueWeight = investigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight,
                AnalysisTechniqueHidden = investigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueHidden,
                AnalysisTechniqueWeight = investigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight,
                //
                EthicalConsiderationHidden = investigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsHidden,
                EthicalConsiderationWeight = investigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight,
                //
                FieldWorkHidden = investigationConvocation.InvestigationConvocationRequirement.FieldWorkHidden,
                FieldWorkWeight = investigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight,

                //
                ExpectedResultsHidden = investigationConvocation.InvestigationConvocationRequirement.ExpectedResultsHidden,
                ExpectedResultsWeight = investigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight,
                BibliographicReferencesHidden = investigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesHidden,
                BibliographicReferencesWeight = investigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight,
                //
                ThesisDevelopmentHidden = investigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentHidden,
                ThesisDevelopmentWeight = investigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight,
                PublishedArticleHidden = investigationConvocation.InvestigationConvocationRequirement.PublishedArticleHidden,
                PublishedArticleWeight = investigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight,
                BroadcastArticleHidden = investigationConvocation.InvestigationConvocationRequirement.BroadcastArticleHidden,
                BroadcastArticleWeight = investigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight,
                ProcessDevelopmentHidden = investigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentHidden,
                ProcessDevelopmentWeight = investigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight,
                ProjectDurationHidden = investigationConvocation.InvestigationConvocationRequirement.ProjectDurationHidden,
                ProjectDurationWeight = investigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight,
                PostulationAttachmentFilesHidden = investigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesHidden,
                PostulationAttachmentFilesWeight = investigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight,
                TeamMemberUserHidden = investigationConvocation.InvestigationConvocationRequirement.TeamMemberUserHidden,
                TeamMemberUserWeight = investigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight,
                ExternalMemberHidden = investigationConvocation.InvestigationConvocationRequirement.ExternalMemberHidden,
                ExternalMemberWeight = investigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight,
                QuestionsHidden = investigationConvocation.InvestigationConvocationRequirement.QuestionsHidden,
                QuestionsWeight = investigationConvocation.InvestigationConvocationRequirement.QuestionsWeight,
                SumPercentage = (
                investigationConvocation.InvestigationConvocationRequirement.InvestigationTypeWeight +
                investigationConvocation.InvestigationConvocationRequirement.ExternalEntityWeight +
                investigationConvocation.InvestigationConvocationRequirement.BudgetWeight +
                investigationConvocation.InvestigationConvocationRequirement.InvestigationPatternWeight +
                investigationConvocation.InvestigationConvocationRequirement.AreaWeight +
                investigationConvocation.InvestigationConvocationRequirement.FacultyWeight +
                investigationConvocation.InvestigationConvocationRequirement.CareerWeight +
                investigationConvocation.InvestigationConvocationRequirement.ResearchCenterWeight +
                investigationConvocation.InvestigationConvocationRequirement.FinancingWeight +
                investigationConvocation.InvestigationConvocationRequirement.MainLocationWeight +
                investigationConvocation.InvestigationConvocationRequirement.ExecutionPlaceWeight +
                researchLineCategoryRequirementsSum +
                investigationConvocation.InvestigationConvocationRequirement.ProjectTitleWeight +
                investigationConvocation.InvestigationConvocationRequirement.ProblemDescriptionWeight +
                investigationConvocation.InvestigationConvocationRequirement.GeneralGoalWeight +
                investigationConvocation.InvestigationConvocationRequirement.ProblemFormulationWeight +
                investigationConvocation.InvestigationConvocationRequirement.SpecificGoalWeight +
                investigationConvocation.InvestigationConvocationRequirement.JustificationWeight +
                investigationConvocation.InvestigationConvocationRequirement.TheoreticalFundamentWeight +
                investigationConvocation.InvestigationConvocationRequirement.ProblemRecordWeight +
                investigationConvocation.InvestigationConvocationRequirement.HypothesisWeight +
                investigationConvocation.InvestigationConvocationRequirement.VariableWeight +
                investigationConvocation.InvestigationConvocationRequirement.MethodologyTypeWeight +
                investigationConvocation.InvestigationConvocationRequirement.MethodologyDescriptionWeight +
                investigationConvocation.InvestigationConvocationRequirement.PopulationWeight +
                investigationConvocation.InvestigationConvocationRequirement.InformationCollectionTechniqueWeight +
                investigationConvocation.InvestigationConvocationRequirement.AnalysisTechniqueWeight +
                //
                investigationConvocation.InvestigationConvocationRequirement.EthicalConsiderationsWeight +
                //
                investigationConvocation.InvestigationConvocationRequirement.FieldWorkWeight +
                investigationConvocation.InvestigationConvocationRequirement.ThesisDevelopmentWeight +
                //
                investigationConvocation.InvestigationConvocationRequirement.ExpectedResultsWeight +
                investigationConvocation.InvestigationConvocationRequirement.BibliographicReferencesWeight +
                //
                investigationConvocation.InvestigationConvocationRequirement.PublishedArticleWeight +
                investigationConvocation.InvestigationConvocationRequirement.BroadcastArticleWeight +
                investigationConvocation.InvestigationConvocationRequirement.ProcessDevelopmentWeight +
                investigationConvocation.InvestigationConvocationRequirement.ProjectDurationWeight +
                investigationConvocation.InvestigationConvocationRequirement.PostulationAttachmentFilesWeight +
                investigationConvocation.InvestigationConvocationRequirement.TeamMemberUserWeight +
                investigationConvocation.InvestigationConvocationRequirement.ExternalMemberWeight +
                investigationConvocation.InvestigationConvocationRequirement.QuestionsWeight)
            };

            var researchLineCategoryRequirements = await _context.ResearchLineCategories
                .Select(x => new ResearchLineCategoryRequirementViewModel
                {
                    ResearchLineCategoryId = x.Id,
                    ResearchLineCategoryRequirementId = x.ResearchLineCategoryRequirements
                        .Where(y => y.ResearchLineCategoryId == x.Id && y.InvestigationConvocationRequirementId == investigationConvocation.InvestigationConvocationRequirement.Id)
                        .Select(y => y.Id)
                        .FirstOrDefault(),
                    Name = x.Name,
                    Code = x.Code,
                    Hidden = x.ResearchLineCategoryRequirements
                        .Where(y => y.ResearchLineCategoryId == x.Id && y.InvestigationConvocationRequirementId == investigationConvocation.InvestigationConvocationRequirement.Id)
                        .Select(y => y.Hidden)
                        .FirstOrDefault(),
                    Weight = x.ResearchLineCategoryRequirements
                        .Where(y => y.ResearchLineCategoryId == x.Id && y.InvestigationConvocationRequirementId == investigationConvocation.InvestigationConvocationRequirement.Id)
                        .Select(y => y.Weight)
                        .FirstOrDefault(),
                    ResearchLineCategoryLines = x.ResearchLines
                            .Select(y => new ResearchLineCategoryLineViewModel 
                            {
                                Id = y.Id,
                                Name = y.Name,
                                Code = y.Code
                            })
                            .ToList()
                })
                .ToListAsync();

            Input.ResearchLineCategoryRequirements = researchLineCategoryRequirements;

            return Page();
        }

        /// <summary>
        /// Enviar la data del formulario ingresada.
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> OnPost()
        {
            var convocationrequirement = await _context.InvestigationConvocationRequirements
                .Include(x=>x.ResearchLineCategoryRequirements)
                .Where(x => x.Id == Input.InvestigationConvocationRequirementId).FirstOrDefaultAsync();            
            
            if (convocationrequirement == null)
            {
                return RedirectToPage("/InvestigationConvocationPage/Index");
            }

            int sumWeights = 
                Input.InvestigationTypeWeight +
                Input.ExternalEntityWeight +
                Input.BudgetWeight +
                Input.InvestigationPatternWeight +
                Input.AreaWeight +
                Input.FacultyWeight +
                Input.CareerWeight +
                Input.ResearchCenterWeight +
                Input.FinancingWeight +
                Input.MainLocationWeight +
                Input.ExecutionPlaceWeight +
                Input.ResearchLineCategoryRequirements.DefaultIfEmpty().Sum(x => x.Weight) +

                Input.ProjectTitleWeight +
                Input.ProblemDescriptionWeight +
                Input.GeneralGoalWeight +
                Input.ProblemFormulationWeight +
                Input.SpecificGoalWeight +
                Input.JustificationWeight +

                Input.TheoreticalFundamentWeight +
                Input.ProblemRecordWeight +
                Input.HypothesisWeight +
                Input.VariableWeight +

                Input.MethodologyTypeWeight +
                Input.MethodologyDescriptionWeight +
                Input.PopulationWeight +
                Input.InformationCollectionTechniqueWeight +
                Input.AnalysisTechniqueWeight +
                //
                Input.EthicalConsiderationWeight+
                //
                Input.FieldWorkWeight +

                //
                Input.ExpectedResultsWeight+
                Input.BibliographicReferencesWeight+

                //

                Input.ThesisDevelopmentWeight +
                Input.PublishedArticleWeight +
                Input.BroadcastArticleWeight +
                Input.ProcessDevelopmentWeight +

                Input.TeamMemberUserWeight +
                Input.ExternalMemberWeight +

                Input.ProjectDurationWeight +
                Input.PostulationAttachmentFilesWeight +

                Input.QuestionsWeight;
                
            if ( sumWeights > 100)
            {
                return new BadRequestObjectResult("La Suma de los Porcentajes No Pueden Exceder al 100%");
            }
            var researchLineCategoryRequirements = Input.ResearchLineCategoryRequirements.Where(x => !x.Hidden).ToList();
           
            //BORRAR CATEGORIAS QUE EXISTEN
            if (convocationrequirement.ResearchLineCategoryRequirements.Count() > 0)
            {
                _context.ResearchLineCategoryRequirements.RemoveRange(convocationrequirement.ResearchLineCategoryRequirements);
            }


            for (int i = 0; i < researchLineCategoryRequirements.Count(); i++)
            {
                var researchLineCategoryRequirement = new ResearchLineCategoryRequirement
                {
                    Hidden = researchLineCategoryRequirements[i].Hidden,
                    Weight = researchLineCategoryRequirements[i].Weight,
                    InvestigationConvocationRequirementId = Input.InvestigationConvocationRequirementId,
                    ResearchLineCategoryId = researchLineCategoryRequirements[i].ResearchLineCategoryId
                };
                convocationrequirement.ResearchLineCategoryRequirements.Add(researchLineCategoryRequirement);
            }

            //
            convocationrequirement.InvestigationTypeHidden = Input.InvestigationTypeHidden;
            convocationrequirement.InvestigationTypeWeight = Input.InvestigationTypeWeight;
            //
            convocationrequirement.ExternalEntityHidden = Input.ExternalEntityHidden;
            convocationrequirement.ExternalEntityWeight = Input.ExternalEntityWeight;
            //
            convocationrequirement.BudgetHidden = Input.BudgetHidden;
            convocationrequirement.BudgetWeight = Input.BudgetWeight;
            //
            convocationrequirement.InvestigationPatternHidden = Input.InvestigationPatternHidden;
            convocationrequirement.InvestigationPatternWeight = Input.InvestigationPatternWeight;
            //
            convocationrequirement.AreaHidden = Input.AreaHidden;
            convocationrequirement.AreaWeight = Input.AreaWeight;
            //
            convocationrequirement.FacultyHidden = Input.FacultyHidden;
            convocationrequirement.FacultyWeight = Input.FacultyWeight;
            //
            convocationrequirement.CareerHidden = Input.CareerHidden;
            convocationrequirement.CareerWeight = Input.CareerWeight;
            //
            convocationrequirement.ResearchCenterHidden = Input.ResearchCenterHidden;
            convocationrequirement.ResearchCenterWeight = Input.ResearchCenterWeight;
            //
            convocationrequirement.FinancingHidden = Input.FinancingHidden;
            convocationrequirement.FinancingWeight = Input.FinancingWeight;
            //
            convocationrequirement.MainLocationHidden = Input.MainLocationHidden;
            convocationrequirement.MainLocationWeight = Input.MainLocationWeight;
            //
            convocationrequirement.ExecutionPlaceHidden = Input.ExecutionPlaceHidden;
            convocationrequirement.ExecutionPlaceWeight = Input.ExecutionPlaceWeight;            
            //
            convocationrequirement.ProjectTitleHidden = Input.ProjectTitleHidden;
            convocationrequirement.ProjectTitleWeight = Input.ProjectTitleWeight;
            //
            convocationrequirement.ProblemDescriptionHidden = Input.ProblemDescriptionHidden;
            convocationrequirement.ProblemDescriptionWeight = Input.ProblemDescriptionWeight;
            //
            convocationrequirement.GeneralGoalHidden = Input.GeneralGoalHidden;
            convocationrequirement.GeneralGoalWeight = Input.GeneralGoalWeight;
            //
            convocationrequirement.ProblemFormulationHidden = Input.ProblemFormulationHidden;
            convocationrequirement.ProblemFormulationWeight = Input.ProblemFormulationWeight;
            //
            convocationrequirement.SpecificGoalHidden = Input.SpecificGoalHidden;
            convocationrequirement.SpecificGoalWeight = Input.SpecificGoalWeight;
            //
            convocationrequirement.JustificationHidden = Input.JustificationHidden;
            convocationrequirement.JustificationWeight = Input.JustificationWeight;
            //
            convocationrequirement.TheoreticalFundamentHidden = Input.TheoreticalFundamentHidden;
            convocationrequirement.TheoreticalFundamentWeight = Input.TheoreticalFundamentWeight;
            //
            convocationrequirement.ProblemRecordHidden = Input.ProblemRecordHidden;
            convocationrequirement.ProblemRecordWeight = Input.ProblemRecordWeight;
            //
            convocationrequirement.HypothesisHidden = Input.HypothesisHidden;
            convocationrequirement.HypothesisWeight = Input.HypothesisWeight;
            //
            convocationrequirement.VariableHidden = Input.VariableHidden;
            convocationrequirement.VariableWeight = Input.VariableWeight;
            //
            convocationrequirement.MethodologyTypeHidden = Input.MethodologyTypeHidden;
            convocationrequirement.MethodologyTypeWeight = Input.MethodologyTypeWeight;
            //
            convocationrequirement.MethodologyDescriptionHidden = Input.MethodologyDescriptionHidden;
            convocationrequirement.MethodologyDescriptionWeight = Input.MethodologyDescriptionWeight;
            //
            convocationrequirement.PopulationHidden = Input.PopulationHidden;
            convocationrequirement.PopulationWeight = Input.PopulationWeight;
            //
            convocationrequirement.InformationCollectionTechniqueHidden = Input.InformationCollectionTechniqueHidden;
            convocationrequirement.InformationCollectionTechniqueWeight = Input.InformationCollectionTechniqueWeight;
            //
            convocationrequirement.AnalysisTechniqueHidden = Input.AnalysisTechniqueHidden;
            convocationrequirement.AnalysisTechniqueWeight = Input.AnalysisTechniqueWeight;
            //
            convocationrequirement.EthicalConsiderationsHidden = Input.EthicalConsiderationHidden;
            convocationrequirement.EthicalConsiderationsWeight = Input.EthicalConsiderationWeight;
            //
            convocationrequirement.FieldWorkHidden = Input.FieldWorkHidden;
            convocationrequirement.FieldWorkWeight = Input.FieldWorkWeight;
            //
            convocationrequirement.TeamMemberUserHidden = Input.TeamMemberUserHidden;
            convocationrequirement.TeamMemberUserWeight = Input.TeamMemberUserWeight;
            //
            convocationrequirement.ExternalMemberHidden = Input.ExternalMemberHidden;
            convocationrequirement.ExternalMemberWeight = Input.ExternalMemberWeight;
            //

            //
            convocationrequirement.ExpectedResultsHidden = Input.ExpectedResultsHidden;
            convocationrequirement.ExpectedResultsWeight = Input.ExpectedResultsWeight;
            convocationrequirement.BibliographicReferencesHidden = Input.ExpectedResultsHidden;
            convocationrequirement.BibliographicReferencesWeight = Input.BibliographicReferencesWeight;
            //
            convocationrequirement.ThesisDevelopmentHidden = Input.ThesisDevelopmentHidden;
            convocationrequirement.ThesisDevelopmentWeight = Input.ThesisDevelopmentWeight;
            //
            convocationrequirement.PublishedArticleHidden = Input.PublishedArticleHidden;
            convocationrequirement.PublishedArticleWeight = Input.PublishedArticleWeight;
            //
            convocationrequirement.BroadcastArticleHidden = Input.BroadcastArticleHidden;
            convocationrequirement.BroadcastArticleWeight = Input.BroadcastArticleWeight;
            //
            convocationrequirement.ProcessDevelopmentHidden = Input.ProcessDevelopmentHidden;
            convocationrequirement.ProcessDevelopmentWeight = Input.ProcessDevelopmentWeight;
            // 
            convocationrequirement.ProjectDurationHidden = Input.ProjectDurationHidden;
            convocationrequirement.ProjectDurationWeight = Input.ProjectDurationWeight;
            //
            convocationrequirement.PostulationAttachmentFilesHidden = Input.PostulationAttachmentFilesHidden;
            convocationrequirement.PostulationAttachmentFilesWeight = Input.PostulationAttachmentFilesWeight;
            //
            convocationrequirement.QuestionsHidden = Input.QuestionsHidden;
            convocationrequirement.QuestionsWeight = Input.QuestionsWeight;


            await _context.SaveChangesAsync();
            return new OkResult();

        }
    }
}
