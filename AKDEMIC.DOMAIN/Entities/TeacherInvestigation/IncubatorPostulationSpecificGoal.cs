using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorPostulationSpecificGoal : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Objetivos Specificos - Sera parte de los componentes en el cronograma
        public Guid Id { get; set; }
        public Guid IncubatorPostulationId { get; set; }
        public IncubatorPostulation IncubatorPostulation { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public ICollection<IncubatorPostulationActivity> IncubatorPostulationActivities { get; set; }
    }
}
