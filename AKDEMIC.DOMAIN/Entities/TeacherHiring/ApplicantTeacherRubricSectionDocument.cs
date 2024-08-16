using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ApplicantTeacherRubricSectionDocument : IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid ConvocationRubricSectionId { get; set; }
        public Guid ApplicantTeacherId { get; set; }
        public ConvocationRubricSection ConvocationRubricSection { get; set; }
        public ApplicantTeacher ApplicantTeacher { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}
