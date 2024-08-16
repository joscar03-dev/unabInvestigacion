using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationAnswerSpecifications
{
    public sealed class ConvocationAnswerFilterSpecification : Specification<ConvocationAnswer>
    {
        public ConvocationAnswerFilterSpecification(Guid convocationQuestionId)
        {
            Query.Where(x => x.ConvocationQuestionId == convocationQuestionId);
        }

        public ConvocationAnswerFilterSpecification(List<Guid> convocationQuestionsId)
        {
            Query.Where(x => convocationQuestionsId.Contains(x.ConvocationQuestionId));
        }

    }
}
