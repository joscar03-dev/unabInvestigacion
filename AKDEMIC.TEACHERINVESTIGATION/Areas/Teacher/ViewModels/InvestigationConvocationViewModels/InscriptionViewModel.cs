using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels
{
    public class InscriptionViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }

        public bool TeamMemberUserHidden { get; set; }
        public int TeamMemberUserWeight { get; set; }

        public bool ExternalMemberHidden { get; set; }
        public int ExternalMemberWeight { get; set; }

        public bool ProjectDurationHidden { get; set; }
        public int ProjectDurationWeight { get; set; }

        public bool PostulationAttachmentFilesHidden { get; set; }
        public int PostulationAttachmentFilesWeight { get; set; }

        public bool QuestionsHidden { get; set; }
        public int QuestionsWeight { get; set; }

        public bool MainLocationHidden { get; set; }

        public int MainLocationWeight { get; set; }
        public string ProjectStateText { get; set; }
        public int ProjectState { get; set; }
        public string ProgressStateText { get; set; }
        public int ProgressState { get; set; }
        public string ReviewStateText { get; set; }
        public int ReviewState { get; set; }

        public List<InvestigationQuestionViewModel> InvestigationQuestions { get; set; }
    }

    public class InvestigationQuestionViewModel
    {
        public Guid Id { get; set; }
        public string QuestionDescription { get; set; }
        public int Type { get; set; }
        public bool IsRequired { get; set; }
        public string AnswerDescription { get; set; }

        public Guid? UniqueAnswer { get; set; }
        public List<InvestigationAnswerViewModel> InvestigationAnswers { get; set; }
    }

    public class InvestigationAnswerViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }

    public class PostulantDocumentsViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string TechnicalReportPath { get; set; }
        public IFormFile TechnicalReport { get; set; }

        public string FinancialReportPath { get; set; }
        public IFormFile FinancialReport { get; set; }
    }

    #region forms dentro del formulario de inscripcion
    //1.Datos Generales
    public class GeneralInformationViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public Guid InvestigationConvocationId { get; set; }

        public Guid? InvestigationTypeId { get; set; }
        public bool InvestigationTypeHidden { get; set; }
        public int InvestigationTypeWeight { get; set; }

        public Guid? ExternalEntityId { get; set; }
        public bool ExternalEntityHidden { get; set; }
        public int ExternalEntityWeight { get; set; }

        public int? Budget { get; set; }
        public bool BudgetHidden { get; set; }
        public int BudgetWeight { get; set; }

        public Guid? InvestigationPatternId { get; set; } //Forma de Investigación
        public bool InvestigationPatternHidden { get; set; } //Forma de Investigación
        public int InvestigationPatternWeight { get; set; }

        public Guid? InvestigationAreaId { get; set; }
        public bool InvestigationAreaHidden { get; set; }
        public int InvestigationAreaWeight { get; set; }

        public Guid? FacultyId { get; set; }
        public bool FacultyHidden { get; set; }
        public int FacultyWeight { get; set; }
        public string FacultyText { get; set; }

        public Guid? CareerId { get; set; }
        public bool CareerHidden { get; set; }
        public int CareerWeight { get; set; }
        public string CareerText { get; set; }

        public Guid? ResearchCenterId { get; set; }
        public bool ResearchCenterHidden { get; set; } //Centro de Investigación
        public int ResearchCenterWeight { get; set; } //Centro de Investigación

        public Guid? FinancingInvestigationId { get; set; } //Financiamiento
        public bool FinancingHidden { get; set; } //Financiamiento
        public int FinancingWeight { get; set; } //Financiamiento
        public bool ExecutionPlaceHidden { get; set; }

        public bool MainLocationHidden { get; set; }

        public int MainLocationWeight { get; set; }

        public string MainLocation { get; set; }

        //Categorias de Investigación
        public List<ResearchLineCategoryRequirmentViewModel> ResearchLineCategoryRequirments { get; set; }
    }

    public class ExecutionPlaceViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public Guid? DepartmentId { get; set; }
        public string DepartmentText { get; set; }

        public Guid? ProvinceId { get; set; }
        public string ProvinceText { get; set; }

        public Guid? DistrictId { get; set; }
        public string DistrictText { get; set; }
    }

    public class ResearchLineCategoryRequirmentViewModel
    {
        public Guid Id { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }
        public Guid ResearchLineCategoryId { get; set; }
        public string ResearchLineCategoryName { get; set; }
        public Guid? ResearchLineSelectedId { get; set; } //la opcion seleccionada


        public int Weight { get; set; }
        public bool Hidden { get; set; }
        public List<ResearchLineViewModel> ResearchLines { get; set; }
    } 

    public class ResearchLineViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


    //2.Descripción del Problema

    public class ProblemDescriptionViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }

        public string ProjectTitle { get; set; }
        public bool ProjectTitleHidden { get; set; }
        public int ProjectTitleWeight { get; set; }

        public string ProblemDescription { get; set; }
        public bool ProblemDescriptionHidden { get; set; }
        public int ProblemDescriptionWeight { get; set; }

        public string GeneralGoal { get; set; }
        public bool GeneralGoalHidden { get; set; }
        public int GeneralGoalWeight { get; set; }

        public string ProblemFormulation { get; set; }
        public bool ProblemFormulationHidden { get; set; }
        public int ProblemFormulationWeight { get; set; }

        public string SpecificGoal { get; set; }
        public bool SpecificGoalHidden { get; set; }
        public int SpecificGoalWeight { get; set; }

        public string Justification { get; set; }
        public bool JustificationHidden { get; set; }
        public int JustificationWeight { get; set; }
    }
    

    //3.Marco de Referencia
    public class MarkReferenceViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }

        public string TheoreticalFundament { get; set; }
        public bool TheoreticalFundamentHidden { get; set; }
        public int TheoreticalFundamentWeight { get; set; }

        public string ProblemRecord { get; set; }
        public bool ProblemRecordHidden { get; set; }
        public int ProblemRecordWeight { get; set; }

        public string Hypothesis { get; set; }
        public bool HypothesisHidden { get; set; }
        public int HypothesisWeight { get; set; }

        public string Variable { get; set; }
        public bool VariableHidden { get; set; }
        public int VariableWeight { get; set; }

    }

    //4.Metodologia
    public class MethodologyViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }

        public Guid? MethodologyTypeId { get; set; }
        public bool MethodologyTypeHidden { get; set; }
        public int MethodologyTypeWeight { get; set; }

        public string MethodologyDescription { get; set; }
        public bool MethodologyDescriptionHidden { get; set; }
        public int MethodologyDescriptionWeight { get; set; }

        public string Population { get; set; }
        public bool PopulationHidden { get; set;}
        public int PopulationWeight { get; set; }

        public string InformationCollectionTechnique { get; set; }
        public bool InformationCollectionTechniqueHidden { get; set; }
        public int InformationCollectionTechniqueWeight { get; set; }

        public string AnalysisTechnique { get; set; }
        public bool AnalysisTechniqueHidden { get; set; }
        public int AnalysisTechniqueWeight { get; set; }

        //
        public string EthicalConsiderations { get; set; }
        public bool EthicalConsiderationsHidden { get; set; }
        public int EthicalConsiderationsWeight { get; set; }
        //
        public string FieldWorkUrl { get; set; }
        public bool FieldWorkHidden { get; set; }
        public int FieldWorkWeight { get; set; }
        public IFormFile FieldWork { get; set; }
    }

    //5.Resultados esperados
    public class ExpectedResultViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }


        //
        public string ExpectedResults { get; set; }
        public bool ExpectedResultsHidden { get; set; }
        public int ExpectedResultsWeight { get; set; }

        public string BibliographicReferences { get; set; }
        public bool BibliographicReferencesHidden { get; set; }
        public int BibliographicReferencesWeight { get; set; }
        //
        public string ThesisDevelopment { get; set; }
        public bool ThesisDevelopmentHidden { get; set; }
        public int ThesisDevelopmentWeight { get; set; }

        public string PublishedArticle { get; set; }
        public bool PublishedArticleHidden { get; set; }
        public int PublishedArticleWeight { get; set; }

        public string BroadcastArticle { get; set; } //Difusión en congreso nacional 
        public bool BroadcastArticleHidden { get; set; }
        public int BroadcastArticleWeight { get; set; }

        public string ProcessDevelopment { get; set; }
        public bool ProcessDevelopmentHidden { get; set; }
        public int ProcessDevelopmentWeight { get; set; }
    }
    //6.Equipo de trabajo
    public class TeamMemberSaveViewModel 
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string ResearcherUserId { get; set; }
        public Guid ResearcherUserRoleId { get; set; }
        public IFormFile CvFile{ get; set; }
        public string Objectives { get; set; }
    }

    public class TeamMemberViewModel
    {
        public Guid Id { get; set; }
        public string ResearcherUserFullName{ get; set; }
        public string ResearcherUserRole { get; set; }
        public string CvFileUrl { get; set; }
        public int TeamMemberUserWeight { get; set; }
    }

    public class ExternalMemberViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Dni { get; set; }
        public string Description { get; set; } //Descripción
        public string InstitutionOrigin { get; set; } //Institución de procedencia
        public string Profession { get; set; } //Profesión
        public IFormFile CvFile { get; set; } //CVFil
        public string Objectives { get; set; } //Objetivos
        public string Address { get; set; } //Dirección
        public string PhoneNumber { get; set; } //Celular
    }

    //7.Anexos adjuntos
    public class AnnexedFileModalViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string AnnexedFileName { get; set; }
        public IFormFile AnnexedFileDocument { get; set; }
        public int PostulationAttachmentFilesWeight { get; set; }
    }

    public class AnnexedFileViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public int? ProjectDuration { get; set; }
        public int ProjectDurationWeight { get; set; }
    }

    //8.AdditionalQuestion
    public class AdditionalQuestionViewModel
    {
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }

        public bool QuestionsHidden { get; set; }
        public int QuestionsWeight { get; set; }

        public List<InvestigationQuestionViewModel> InvestigationQuestions { get; set; }
    }


    #endregion

    public class InscriptionProgressDataViewModel
    {
        public int GeneralInformationPercentage { get; set; }
        public int ProblemDescriptionPercentage { get; set; }
        public int MarkReferencePercentage { get; set; }
        public int MethodologyPercentage { get; set; }
        public int ExpectedResultPercentage { get; set; }
        public int TeamMemberPercentage { get; set; }
        public int AnnexFilesPercentage { get; set; }
        public int AdditionalQuestionsPercentage { get; set; }
        public int ProgressBarPercentage { get; set; }
    }
}
