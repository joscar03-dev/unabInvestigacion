using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PostulantObservation : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public int State { get; set; }
        public string Description { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }
    }
}
