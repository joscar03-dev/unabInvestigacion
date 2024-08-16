using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorPostulationActivity : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IncubatorPostulationSpecificGoalId { get; set; }
        public IncubatorPostulationSpecificGoal IncubatorPostulationSpecificGoal { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public ICollection<IncubatorPostulationActivityMonth> IncubatorPostulationActivityMonths { get; set; }
    }
}
