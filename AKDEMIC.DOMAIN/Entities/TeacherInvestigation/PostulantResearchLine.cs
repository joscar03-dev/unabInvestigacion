using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PostulantResearchLine : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
        public Guid ResearchLineId { get; set; }

        public ResearchLine ResearchLine { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }
    }
}
