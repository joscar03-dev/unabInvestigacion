using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationRubricItem : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [Required]
        public decimal MaxScore { get; set; }
        public Guid ConvocationRubricSectionId { get; set; }
        public ConvocationRubricSection ConvocationRubricSection { get; set; }
    }
}
