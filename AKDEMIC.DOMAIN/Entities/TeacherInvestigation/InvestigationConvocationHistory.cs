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
    public class InvestigationConvocationHistory : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid InvestigationConvocationId { get; set; }
        public string UserId { get; set; }
        public DateTime OldEndDate { get; set; }
        public DateTime NewEndDate { get; set; }

        public string ResolutionUrl { get; set; }

        public ApplicationUser User { get; set; }
        public InvestigationConvocation InvestigationConvocation { get; set; }
    }
}
