using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class EventParticipant : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public Event Event { get; set; }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string MaternalSurname { get; set; }

        [MaxLength(250)]
        public string PaternalSurname { get; set; }
        public DateTime BirthDate { get; set; }

        [MaxLength(8)]
        public string Dni { get; set; }
        public string Email { get; set; }
        public string University { get; set; }
        public string PhoneNumber { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
