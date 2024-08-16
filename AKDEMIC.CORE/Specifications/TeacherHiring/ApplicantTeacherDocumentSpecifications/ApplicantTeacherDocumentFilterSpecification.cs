using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherDocumentSpecifications
{
    public sealed class ApplicantTeacherDocumentFilterSpecification : Specification<ApplicantTeacherDocument>
    {
        public ApplicantTeacherDocumentFilterSpecification(Guid applicantTeacherId)
        {
            Query.Where(x => x.ApplicantTeacherId == applicantTeacherId);
        }
    }
}
