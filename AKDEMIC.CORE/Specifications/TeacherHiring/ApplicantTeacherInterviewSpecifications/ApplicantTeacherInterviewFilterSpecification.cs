using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherInterviewSpecifications
{
    public sealed class ApplicantTeacherInterviewFilterSpecification : Specification<ApplicantTeacherInterview>
    {
        public ApplicantTeacherInterviewFilterSpecification(Guid applicantTeacherId, byte type)
        {
            Query.Where(x => x.ApplicantTeacherId == applicantTeacherId && x.Type == type);
        }
    }
}
