using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorPostulationActivityMonth : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid IncubatorPostulationActivityId { get; set; }
        public IncubatorPostulationActivity IncubatorPostulationActivity { get; set; }
        public int MonthNumber { get; set; }
    }
}
