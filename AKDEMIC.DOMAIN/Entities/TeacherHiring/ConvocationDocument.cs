using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationDocument : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }

        public byte Type { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
        public ICollection<ApplicantTeacherDocument> ApplicantTeacherDocuments { get; set; }
    }
}
