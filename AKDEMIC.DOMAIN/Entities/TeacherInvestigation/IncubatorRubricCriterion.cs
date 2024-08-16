using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorRubricCriterion : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public Guid IncubatorRubricSectionId { get; set; }
        public IncubatorRubricSection IncubatorRubricSection { get; set; }

        public ICollection<IncubatorRubricLevel> IncubatorRubricLevels { get; set; }
    }
}
