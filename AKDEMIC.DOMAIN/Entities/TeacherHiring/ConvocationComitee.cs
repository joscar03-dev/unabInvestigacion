using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationComitee : BaseEntity, ITimestamp, IAggregateRoot
    {
        [Key]
        public string UserId { get; set; }
        [Key]
        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }
        public ApplicationUser User { get; set; }
        [Required]
        public string ApplicationRoleId { get; set; }
        public ApplicationRole ApplicationRole { get; set; }

        public Guid AcademicDeparmentId { get; set; }
        public string AcademicDepartmentText { get; set; }
    }
}
