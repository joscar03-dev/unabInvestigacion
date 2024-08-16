using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationProjectReport : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string ReportUrl { get; set; }
        public DateTime? LastEmailSendedDate { get; set; }
        public Guid InvestigationProjectId { get; set; }
        public InvestigationProject InvestigationProject { get; set; }
    }
}
