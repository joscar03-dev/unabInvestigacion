using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationQuestion : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public byte StaticType { get; set; }
        public byte Type { get; set; }
        [Required]
        [StringLength(1000)]
        public string Description { get; set; }
        public Guid ConvocationSectionId { get; set; }
        public ConvocationSection ConvocationSection { get; set; }
        public ICollection<ConvocationAnswer> ConvocationAnswers { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
    }
}
