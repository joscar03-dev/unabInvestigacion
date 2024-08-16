using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class Event : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public string PicturePath { get; set; }
        public string VideoUrl { get; set; }
        public decimal Cost { get; set; }
        public string Organizer { get; set; }
        public Guid? UnitId { get; set; }
        public Unit Unit { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
