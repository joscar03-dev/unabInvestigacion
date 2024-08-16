using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationConvocationPostulant : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Guid InvestigationConvocationId { get; set; }
        public InvestigationConvocation InvestigationConvocation { get; set; }

        public string ResolutionDocumentPath { get; set; } //Resolución de Aceptación por el comite evaluador
        public string MonitorDocumentPath { get; set; } //Documento de aprobacion por parte del monitor
        public string MonitorUserId { get; set; } //Usuario monitor que subio el documento
        public ApplicationUser MonitorUser{ get; set; }

        public Guid? InvestigationTypeId { get; set; }
        public InvestigationType InvestigationType { get; set; }

        public Guid? ExternalEntityId { get; set; }
        public ExternalEntity ExternalEntity { get; set; }

        public int? Budget { get; set; }

        public Guid? InvestigationPatternId { get; set; } //Forma de Investigacion
        public InvestigationPattern InvestigationPattern { get; set; }

        public Guid? InvestigationAreaId { get; set; }
        public InvestigationArea InvestigationArea { get; set; }

        public Guid? FacultyId { get; set; } //Informacion del Api
        public string FacultyText { get; set; }

        public Guid? CareerId { get; set; } //Informacion del Api
        public string CareerText { get; set; }

        public Guid? ResearchCenterId { get; set; } //Centro de Investigación
        public ResearchCenter ResearchCenter { get; set; } //Centro de Investigación

        public Guid? FinancingInvestigationId { get; set; } //Financiamiento
        public FinancingInvestigation FinancingInvestigation { get; set; } //Financiamiento

        public string MainLocation { get; set; } //Localización Principal del Proyecto
        public string ProjectTitle { get; set; }
        public string ProblemDescription { get; set; }
        public string GeneralGoal { get; set; }
        public string ProblemFormulation { get; set; }
        public string SpecificGoal { get; set; }
        public string Justification { get; set; }
        public string TheoreticalFundament { get; set; }
        public string ProblemRecord { get; set; }
        public string Hypothesis { get; set; }
        public string Variable { get; set; }

        public Guid? MethodologyTypeId { get; set; }
        public MethodologyType MethodologyType { get; set; }
        public string MethodologyDescription { get; set; }
        public string Population { get; set; }
        public string InformationCollectionTechnique { get; set; }
        public string AnalysisTechnique { get; set; }
        public string EthicalConsiderations { get; set; } //Consideraciones éticas
        public string FieldWork { get; set; }

        //5. Resultados Esperados
        public string ExpectedResults { get; set; } //Resultados esperados
        public string ThesisDevelopment { get; set; }
        public string PublishedArticle { get; set; }
        public string BroadcastArticle { get; set; } //Difusión en congreso nacional 
        public string ProcessDevelopment { get; set; }
        public string BibliographicReferences { get; set; } //Referencias Bibliograficas

        //6.
        public int? ProjectDuration { get; set; } //Duración del proyecto en meses

        public int ProjectState { get; set; } //Estado del proyecto
        public int ProgressState { get; set; } //Estado de avance
        public int ReviewState { get; set; } //Estado de revision

        public ICollection<PostulantObservation> PostulantObservations { get; set; }
        public ICollection<InvestigationConvocationInquiry> InvestigationConvocationInquiries { get; set; }
        public ICollection<PostulantRubricQualification> PostulantRubricQualifications { get; set; }
        public ICollection<ProgressFileConvocationPostulant> ProgressFileConvocationPostulants { get; set; }
        public ICollection<PostulantTechnicalFile> PostulantTechnicalFiles { get; set; }
        public ICollection<PostulantFinancialFile> PostulantFinancialFiles { get; set; }
        public ICollection<PostulantTeamMemberUser> PostulantTeamMemberUsers { get; set; }
        public ICollection<PostulantExternalMember> PostulantExternalMembers { get; set; }       
        public ICollection<PostulantExecutionPlace> PostulantExecutionPlaces { get; set; }
        public ICollection<PostulantAnnexFile> PostulantAnnexFiles { get; set; }
        public ICollection<PostulantResearchLine> PostulantResearchLines { get; set; }

    }
}
