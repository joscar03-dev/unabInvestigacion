using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels
{
    public class InvestigationConvocationPostulantViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }

        public Guid InvestigationConvocationRequirementId { get; set; }

        public string UserId { get; set; }
        public string InvestigationTypeText { get; set; }

        public string ExternalEntityText { get; set; }

        public int? Budget { get; set; }

        public string InvestigationPatternText { get; set; } //Forma de Investigación
        public string InvestigationAreaText { get; set; }

        public string FacultyText { get; set; }

        public string CareerText { get; set; }

        public string ResearchCenterText { get; set; } //Centro de Investigación

        public string FinancingInvestigationText { get; set; } //Financiamiento

        public string ProjectTitle { get; set; }
        public string GeneralGoal { get; set; }
        public string ProblemFormulation { get; set; }
        public string SpecificGoal { get; set; }
        public string Justification { get; set; }
        public string TheoreticalFundament { get; set; }
        public string ProblemRecord { get; set; }
        public string Hypothesis { get; set; }
        public string Variable { get; set; }

        public string MethodologyTypeCode { get; set; }
        public string MethodologyTypeText { get; set; }

        public string MethodologyDescription { get; set; }

        public string Population { get; set; }
        public string InformationCollectionTechnique { get; set; }
        public string AnalysisTechnique { get; set; }
        //
        public string EthicalConsiderations { get; set; }
        //
        public string FieldWork { get; set; }
        //
        public string ExpectedResults { get; set; }
        public string BibliographicReferences { get; set; }
        //
        public string ThesisDevelopment { get; set; }
        public string PublishedArticle { get; set; }
        public string BroadcastArticle { get; set; } //Difusión en congreso nacional 
        public string ProcessDevelopment { get; set; }

        public int? ProjectDuration { get; set; }

        public decimal ConvocationMinScore { get; set; }
        public bool IsRated { get; set; }
        public decimal RateScore { get; set; }

        public int ProgressPercentage { get; set; }
        public string ProjectStateText { get; set; }
        public int ProjectState { get; set; }
        public string ProgressStateText { get; set; }
        public int ProgressState { get; set; }
        public string ReviewStateText { get; set; }
        public int ReviewState { get; set; }
        public int PercentA { get; set; }

        public bool QuestionsHidden { get; set; }

        public bool MainLocationHidden { get; set; }

        public string MainLocation { get; set; }

        public List<PostulantTeamMemberUserViewModel> PostulantTeamMembers { get; set; }
        public List<PostulantExternalMemberViewModel> PostulantExternalMembers { get; set; }
        public List<PostulantExecutionPlaceViewModel> PostulantExecutionPlaces { get; set; }
        public List<InvestigationQuestionsViewModel> InvestigationQuestions { get; set; }
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
