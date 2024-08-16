using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class ResearchLineCategoryRequirement : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }

        public Guid InvestigationConvocationRequirementId { get; set; }
        public InvestigationConvocationRequirement InvestigationConvocationRequirement { get; set; }

        public Guid ResearchLineCategoryId { get; set; }
        public ResearchLineCategory ResearchLineCategory { get; set; }

        public int Weight { get; set; }
        public bool Hidden { get; set; }

    }
}
