using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationAnswerByUser : IAggregateRoot
    {
        public Guid Id { get; set; }
        
        public Guid ConvocationQuestionId { get; set; }
        public ConvocationQuestion ConvocationQuestion { get; set; }

        [StringLength(500)]
        public string AnswerDescription { get; set; }

        public Guid? ConvocationAnswerId { get; set; }
        public ConvocationAnswer ConvocationAnswer { get; set; }

        public Guid ApplicantTeacherId { get; set; }
        public ApplicantTeacher ApplicantTeacher { get; set; }
    }
}
