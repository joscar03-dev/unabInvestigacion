using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherRubricSectionDocumentSpecifications
{
    public sealed class ApplicantTeacherRubricSectionDocumentFilterSpecification : Specification<ApplicantTeacherRubricSectionDocument>
    {
        public ApplicantTeacherRubricSectionDocumentFilterSpecification(Guid applicantTeacherId)
        {
            Query.Where(x => x.ApplicantTeacherId == applicantTeacherId);
        }
    }
}
