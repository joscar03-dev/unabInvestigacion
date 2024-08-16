using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationSection : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Title { get; set; }
        [StringLength(1000)]
        public string Description { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public ICollection<ConvocationQuestion> ConvocationQuestions { get; set; }
    }
}
