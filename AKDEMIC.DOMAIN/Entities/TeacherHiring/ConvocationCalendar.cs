using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationCalendar : IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [StringLength(400)]
        public string Description { get; set; }
    }
}
