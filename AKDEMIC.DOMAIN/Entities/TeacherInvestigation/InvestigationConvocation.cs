using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationConvocation : BaseEntity, ITimestamp, ISoftDelete, IAggregateRoot
    {
        public Guid Id { get; set; }

        [StringLength(900)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(3000)]
        public string Description { get; set; }

        public string PicturePath { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime InscriptionStartDate { get; set; }

        public DateTime InscriptionEndDate { get; set; }

        [Column(TypeName = "decimal(18, 4)")]
        public decimal MinScore { get; set; }

        public byte State { get; set; }

        public bool AllowInquiries{ get; set; }

        public DateTime? InquiryStartDate { get; set; }

        public DateTime? InquiryEndDate { get; set; }

        public InvestigationConvocationRequirement InvestigationConvocationRequirement { get; set; }

        public ICollection<InvestigationConvocationFile> InvestigationConvocationFiles { get; set; }
        public ICollection<InvestigationConvocationEvaluator> InvestigationConvocationEvaluators { get; set; }
        public ICollection<InvestigationConvocationSupervisor> InvestigationConvocationSupervisors { get; set; }
        public ICollection<EvaluatorCommitteeConvocation> EvaluatorCommitteeConvocations { get; set; }
        public ICollection<CoordinatorMonitorConvocation> CoordinatorMonitorConvocations { get; set; }
        public ICollection<InvestigationConvocationPostulant> InvestigationConvocationPostulants { get; set; }
        public ICollection<MonitorConvocation> MonitorConvocations { get; set; }
        public ICollection<InvestigationConvocationHistory> InvestigationConvocationHistories { get; set; }
    }
}
