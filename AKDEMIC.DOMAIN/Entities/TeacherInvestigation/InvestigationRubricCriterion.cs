using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationRubricCriterion :  BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid InvestigationRubricSectionId { get; set; }
        public InvestigationRubricSection InvestigationRubricSection { get; set; }

        public ICollection<InvestigationRubricLevel> InvestigationRubricLevels { get; set; }
    }
}
