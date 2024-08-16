using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationQuestion : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        
        public bool IsRequired { get; set; }
        [StringLength(500)]
        public int Type { get; set; }
        public string Description { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }
        public InvestigationConvocationRequirement InvestigationConvocationRequirement { get; set; }

        public ICollection<InvestigationAnswer> InvestigationAnswers { get; set; }
        public ICollection<InvestigationAnswerByUser> InvestigationAnswerByUsers { get; set; }
    }
}
