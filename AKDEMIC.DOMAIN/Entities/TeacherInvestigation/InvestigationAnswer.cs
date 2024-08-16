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
    public class InvestigationAnswer : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public Guid InvestigationQuestionId { get; set; }
        public InvestigationQuestion InvestigationQuestion { get; set; }
        //public ICollection<InvestigationConvocationAnswerByUser> InvestigationConvocationAnswerByUser { get; set; }
    }
}
