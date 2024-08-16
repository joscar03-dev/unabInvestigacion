using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationRubricSection : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal MaxSectionScore { get; set; }

        public Guid InvestigationConvocationId { get; set; }
        public InvestigationConvocation InvestigationConvocation { get; set; }

        public ICollection<InvestigationRubricCriterion> InvestigationRubricCriterions { get; set; }
    }
}
