using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationConvocationRequirement : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }

        //DatosGenerales - Ambito de Investigacion
        public bool InvestigationTypeHidden { get; set; }
        public int InvestigationTypeWeight { get; set; }

        public bool ExternalEntityHidden { get; set; }
        public int ExternalEntityWeight { get; set; }

        public bool BudgetHidden { get; set; }
        public int BudgetWeight { get; set; }

        public bool InvestigationPatternHidden { get; set; } //Forma de Investigacion
        public int InvestigationPatternWeight { get; set; }

        public bool AreaHidden { get; set; }
        public int AreaWeight { get; set; }

        public bool FacultyHidden { get; set; }
        public int FacultyWeight { get; set; }

        public bool CareerHidden { get; set; }
        public int CareerWeight { get; set; }

        public bool ResearchCenterHidden { get; set; } //Centro de Investigación
        public int ResearchCenterWeight { get; set; } //Centro de Investigación

        public bool FinancingHidden { get; set; } //Financiamiento
        public int FinancingWeight { get; set; } //Financiamiento

        #region Lugar de ejecución
        public bool MainLocationHidden { get; set; } //Localización Principal del Proyecto
        public int MainLocationWeight { get; set; } //Localización Principal del Proyecto


        public bool ExecutionPlaceHidden { get; set; }
        public int ExecutionPlaceWeight { get; set; }
        #endregion

        public bool ProjectTitleHidden { get; set; }
        public int ProjectTitleWeight { get; set; }

        public bool ProblemDescriptionHidden { get; set; }
        public int ProblemDescriptionWeight { get; set; }

        public bool GeneralGoalHidden { get; set; }
        public int GeneralGoalWeight { get; set; }

        public bool ProblemFormulationHidden { get; set; }
        public int ProblemFormulationWeight { get; set; }

        public bool SpecificGoalHidden { get; set; }
        public int SpecificGoalWeight { get; set; }

        public bool JustificationHidden { get; set; }
        public int JustificationWeight { get; set; }

        public bool TheoreticalFundamentHidden { get; set; }
        public int TheoreticalFundamentWeight { get; set; }

        public bool ProblemRecordHidden { get; set; }
        public int ProblemRecordWeight { get; set; }

        public bool HypothesisHidden { get; set; }
        public int HypothesisWeight { get; set; }

        public bool VariableHidden { get; set; }
        public int VariableWeight { get; set; }

        public bool MethodologyTypeHidden { get; set; }
        public int MethodologyTypeWeight { get; set; }

        public bool MethodologyDescriptionHidden { get; set; }
        public int MethodologyDescriptionWeight { get; set; }

        public bool PopulationHidden { get; set; }
        public int PopulationWeight { get; set; }

        public bool InformationCollectionTechniqueHidden { get; set; }
        public int InformationCollectionTechniqueWeight { get; set; }

        public bool AnalysisTechniqueHidden { get; set; }
        public int AnalysisTechniqueWeight { get; set; }

        public bool EthicalConsiderationsHidden { get; set; }
        public int EthicalConsiderationsWeight { get; set; }

        public bool FieldWorkHidden { get; set; }
        public int FieldWorkWeight { get; set; }

        public bool ExpectedResultsHidden { get; set; } //Resultados esperados
        public int ExpectedResultsWeight { get; set; }

        public bool ThesisDevelopmentHidden { get; set; }
        public int ThesisDevelopmentWeight { get; set; }

        public bool PublishedArticleHidden { get; set; }
        public int PublishedArticleWeight { get; set; }

        public bool BroadcastArticleHidden { get; set; } //Difusión en congreso nacional 
        public int BroadcastArticleWeight { get; set; }

        public bool ProcessDevelopmentHidden { get; set; }
        public int ProcessDevelopmentWeight { get; set; }

        public bool BibliographicReferencesHidden { get; set; } //Referencias Bibliograficas
        public int BibliographicReferencesWeight { get; set; }

        #region Equipo de trabajo
        public bool TeamMemberUserHidden { get; set; }
        public int TeamMemberUserWeight { get; set; }

        public bool ExternalMemberHidden { get; set; }
        public int ExternalMemberWeight { get; set; }
        #endregion

        public bool ProjectDurationHidden { get; set; }
        public int ProjectDurationWeight { get; set; }

        public bool PostulationAttachmentFilesHidden { get; set; } //Archivos de anexos de postulación
        public int PostulationAttachmentFilesWeight { get; set; } //Archivos de anexos de postulación

        public bool QuestionsHidden { get; set; }
        public int QuestionsWeight { get; set; }

        public InvestigationConvocation InvestigationConvocation { get; set; }


        public ICollection<ResearchLineCategoryRequirement> ResearchLineCategoryRequirements { get; set; }

    }
}
