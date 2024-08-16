using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Monitor.ViewModels.InvestigationConvocationPostulantViewModels
{
    public class InvestigationConvocationPostulantViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }

        public Guid InvestigationConvocationRequirementId { get; set; }

        public bool CanUploadMonitorFile { get; set; }
        public string MonitorFileObservation { get; set; }

        public string UserId { get; set; }

        public string InvestigationTypeText { get; set; }
        public bool InvestigationTypeHidden { get; set; }

        public string ExternalEntityText { get; set; }
        public bool ExternalEntityHidden { get; set; }

        public int? Budget { get; set; }
        public bool BudgetHidden { get; set; }

        public string InvestigationPatternText { get; set; } //Forma de Investigación
        public bool InvestigationPatternHidden { get; set; }

        public string InvestigationAreaText { get; set; }
        public bool InvestigationAreaHidden { get; set; }

        public string FacultyCode { get; set; }
        public string FacultyText { get; set; }
        public bool FacultyHidden { get; set; }

        public string CareerCode { get; set; }
        public string CareerText { get; set; }
        public bool CareerHidden { get; set; }

        public string ResearchCenterCode { get; set; } //Centro de Investigación
        public string ResearchCenterText { get; set; } //Centro de Investigación
        public bool ResearchCenterHidden { get; set; }

        public string FinancingInvestigationText { get; set; } //Financiamiento
        public bool FinancingHidden { get; set; } //Financiamiento

        #region Lugar de ejecución
        public bool ExecutionPlaceHidden { get; set; }

        public string MainLocation { get; set; }

        public bool MainLocationHidden { get; set; }
        #endregion


        public string ProjectTitle { get; set; }
        public bool ProjectTitleHidden { get; set; }

        public string ProblemDescription { get; set; }
        public bool ProblemDescriptionHidden { get; set; }

        public string GeneralGoal { get; set; }
        public bool GeneralGoalHidden { get; set; }

        public string ProblemFormulation { get; set; }
        public bool ProblemFormulationHidden { get; set; }

        public string SpecificGoal { get; set; }
        public bool SpecificGoalHidden { get; set; }

        public string Justification { get; set; }
        public bool JustificationHidden { get; set; }

        public string TheoreticalFundament { get; set; }
        public bool TheoreticalFundamentHidden { get; set; }

        public string ProblemRecord { get; set; }
        public bool ProblemRecordHidden { get; set; }

        public string Hypothesis { get; set; }
        public bool HypothesisHidden { get; set; }

        public string Variable { get; set; }
        public bool VariableHidden { get; set; }

        public string MethodologyTypeText { get; set; }
        public bool MethodologyTypeHidden { get; set; }

        public string MethodologyDescription { get; set; }
        public bool MethodologyDescriptionHidden { get; set; }

        public string Population { get; set; }
        public bool PopulationHidden { get; set; }

        public string InformationCollectionTechnique { get; set; }
        public bool InformationCollectionTechniqueHidden { get; set; }

        public string AnalysisTechnique { get; set; }
        public bool AnalysisTechniqueHidden { get; set; }

        //
        public string EthicalConsiderations { get; set; }
        public bool EthicalConsiderationsHidden { get; set; }
        //

        public string FieldWork { get; set; }
        public bool FieldWorkHidden { get; set; }

        //
        public string ExpectedResults { get; set; }
        public bool ExpectedResultsHidden { get; set; }

        public string BibliographicReferences { get; set; }
        public bool BibliographicReferencesHidden { get; set; }
        //

        public string ThesisDevelopment { get; set; }
        public bool ThesisDevelopmentHidden { get; set; }

        public string PublishedArticle { get; set; }
        public bool PublishedArticleHidden { get; set; }

        public string BroadcastArticle { get; set; } //Difusión en congreso nacional
        public bool BroadcastArticleHidden { get; set; } //Difusión en congreso nacional 

        public bool PostulationAttachmentFilesHidden { get; set; }

        public string ProcessDevelopment { get; set; }
        public bool ProcessDevelopmentHidden { get; set; }

        public int? ProjectDuration { get; set; }
        public bool ProjectDurationHidden { get; set; }


        #region Equipo de trabajo
        public bool TeamMemberUserHidden { get; set; }
        public bool ExternalMemberHidden { get; set; }
        #endregion


        public bool QuestionsHidden { get; set; }
        public List<InvestigationQuestionsViewModel> InvestigationQuestions { get; set; }
        public List<PostulantTeamMemberUserViewModel> PostulantTeamMembers { get; set; }
        public List<PostulantExternalMemberViewModel> PostulantExternalMembers { get; set; }
        public List<PostulantExecutionPlaceViewModel> PostulantExecutionPlaces { get; set; }
        public List<ResearchLineCategoryRequirmentViewModel> ResearchLineCategoryRequirments { get; set; }
    }
    public class ResearchLineCategoryRequirmentViewModel
    {
        public string ResearchLineCategoryName { get; set; }
        public string ResearchLineSelected { get; set; } //la opcion seleccionada
        public bool Hidden { get; set; }
    }
    public class PostulantTeamMemberUserViewModel
    {
        public string UserMemberFullName { get; set; }
        public string TeamMemberRole { get; set; }
        public string CvFilePath { get; set; }
        public string Objectives { get; set; }

    }
    public class PostulantExternalMemberViewModel
    {
        public string FullName { get; set; }
        public string Dni { get; set; }
        public string Profession { get; set; }
        public string CvFilePath { get; set; }


    }

    public class PostulantExecutionPlaceViewModel
    {
        public string DepartmentText { get; set; }
        public string ProvinceText { get; set; }
        public string DistrictText { get; set; }
    }

    public class InvestigationQuestionsViewModel
    {
        public Guid Id { get; set; }
        public string QuestionDescription { get; set; }
        public int Type { get; set; }
        public bool IsRequired { get; set; }
        public string AnswerDescription { get; set; }
        public Guid? UniqueAnswer { get; set; }
        public List<InvestigationAnswersViewModel> InvestigationAnswers { get; set; }
    }

    public class InvestigationAnswersViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
}
