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
    public class InvestigationAnswerByUser : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string AnswerDescription { get; set; }

        public Guid InvestigationQuestionId { get; set; }
        public InvestigationQuestion InvestigationQuestion { get; set; }

        public Guid? InvestigationAnswerId { get; set; }
        public InvestigationAnswer InvestigationAnswer { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
