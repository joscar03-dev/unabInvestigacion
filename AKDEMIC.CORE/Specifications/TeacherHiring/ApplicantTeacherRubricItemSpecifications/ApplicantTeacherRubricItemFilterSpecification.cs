using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherRubricItemSpecifications
{
    public sealed class ApplicantTeacherRubricItemFilterSpecification : Specification<ApplicantTeacherRubricItem>
    {
        public ApplicantTeacherRubricItemFilterSpecification(Guid applicantTeacherId, byte sectionType, string evaluatorId = null)
        {
            Query.Where(x => x.ApplicantTeacherId == applicantTeacherId && x.ConvocationRubricItem.ConvocationRubricSection.Type == sectionType);

            if (!string.IsNullOrEmpty(evaluatorId))
                Query.Where(x => x.EvaluatorId == evaluatorId);

        }
    }
}
