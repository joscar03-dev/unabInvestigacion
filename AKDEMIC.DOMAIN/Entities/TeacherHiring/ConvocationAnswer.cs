using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationAnswer : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public Guid ConvocationQuestionId { get; set; }
        public ConvocationQuestion ConvocationQuestion { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUser { get; set; }
    }
}
