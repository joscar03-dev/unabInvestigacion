using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationProject : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
        public Guid? InvestigationProjectTypeId { get; set; }
        public string GanttDiagramUrl { get; set; }
        public string ExecutionAddress { get; set; }
        public string GeneralGoal { get; set; }
        public string SpecificGoal { get; set; }

        public string FinalReportUrl { get; set; }

        public InvestigationProjectType InvestigationProjectType { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }

        public ICollection<InvestigationProjectTask> InvestigationProjectTasks { get; set; }
        public ICollection<InvestigationProjectReport> InvestigationProjectReports { get; set; }
        public ICollection<InvestigationProjectTeamMember> InvestigationProjectTeamMembers { get; set; }
    }
}
