using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ApplicantTeacherDocument : IAggregateRoot
    {
        [Key]
        public Guid ApplicantTeacherId { get; set; }
        [Key]
        public Guid CovocationDocumentId { get; set; }
        public ConvocationDocument ConvocationDocument { get; set; }
        public ApplicantTeacher ApplicantTeacher { get; set; }
        public string Url { get; set; }
    }
}
