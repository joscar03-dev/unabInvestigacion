using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationRubricSection : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(1500)]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MaxScore { get; set; }
        public byte Type { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public List<ConvocationRubricItem> ConvocationRubricItems { get; set; }
    }
}
