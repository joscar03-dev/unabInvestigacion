using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class CoordinatorMonitorConvocation : BaseEntity, ITimestamp, IAggregateRoot
    {
        [Required]
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }

        public InvestigationConvocation InvestigationConvocation { get; set; }
        public ApplicationUser User { get; set; }
    }
}
