using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ApplicantTeacherInterview : IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid ApplicantTeacherId { get; set; }
        public ApplicantTeacher ApplicantTeacher { get; set; }
        public DateTime DateTime { get; set; }
        public string InterviewLink { get; set; }
        public byte Type { get; set; }
        public string Topic { get; set; }
    }
}
