using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ApplicantTeacher : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid ConvocationId { get; set; }
        public ApplicationUser User { get; set; }
        public Convocation Convocation { get; set; }
        public byte Status { get; set; }
        public bool? Valid { get; set; }
        public Guid ConvocationVacancyId { get; set; }
        public string Observation { get; set; }
        public decimal? InterviewScore { get; set; }
        public decimal? MasterClassScore { get; set; }
        public decimal? ExternalEvaluationScore { get; set; }
        public decimal? FinalScore { get; set; }
        public ConvocationVacancy ConvocationVacancy { get; set; }
        public ICollection<ConvocationAnswerByUser> ConvocationAnswerByUsers { get; set; }
        public ICollection<ApplicantTeacherDocument> ApplicantTeacherDocuments { get; set; }
        public ICollection<ApplicantTeacherInterview> ApplicantTeacherInterviews { get; set; }
        public ICollection<ApplicantTeacherRubricSectionDocument> ApplicantTeacherRubricSectionDocuments { get; set; }
    }
}
