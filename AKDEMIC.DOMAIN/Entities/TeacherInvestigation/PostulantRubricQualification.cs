using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PostulantRubricQualification : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }

        public string EvaluatorId { get; set; }
        public ApplicationUser Evaluator { get; set; }

        public Guid InvestigationConvocationPostulantId { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }

        public Guid InvestigationRubricCriterionId { get; set; }
        public InvestigationRubricCriterion InvestigationRubricCriterion { get; set; }

        public decimal Value { get; set; }
    }
}
