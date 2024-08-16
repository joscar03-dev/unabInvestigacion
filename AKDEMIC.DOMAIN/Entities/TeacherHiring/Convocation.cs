using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class Convocation : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public decimal MinScore { get; set; }
        public string Requirements { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal ExternalEvaluationWeight { get; set; }
        public decimal PersonalInterviewWeight { get; set; }
        public decimal MasterClassWeight { get; set; }
        public bool EnabledMasterClass { get; set; }
        public byte Type { get; set; }
        public ICollection<ConvocationVacancy> ConvocationVacancies { get; set; }
        public ICollection<ConvocationComitee> ConvocationComitees { get; set; }
        public ICollection<ConvocationCalendar> ConvocationCalendars { get; set; }
        public ICollection<ConvocationSection> ConvocationSections { get; set; }
        public ICollection<ConvocationDocument> ConvocationDocuments { get; set; }
        public ICollection<ConvocationRubricSection> ConvocationRubricSections { get; set; }
        public ICollection<ApplicantTeacher> ApplicantTeachers { get; set; }
    }
}
