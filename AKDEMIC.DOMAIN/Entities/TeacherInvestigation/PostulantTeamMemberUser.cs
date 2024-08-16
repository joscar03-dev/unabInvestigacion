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
    public class PostulantTeamMemberUser : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string UserId { get; set; }
        public Guid TeamMemberRoleId { get; set; }
        public string CvFilePath { get; set; }
        public string Objectives { get; set; }
        public TeamMemberRole TeamMemberRole { get; set; }
        public ApplicationUser User { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }

    }
}
