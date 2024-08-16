using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationQuestionSpecifications
{
    public sealed class ConvocationQuestionFilterSpecification : Specification<ConvocationQuestion>
    {
        public ConvocationQuestionFilterSpecification(Guid convocationSectionId)
        {
            Query.Where(x => x.ConvocationSectionId == convocationSectionId);
        }

        public ConvocationQuestionFilterSpecification(List<Guid> convocationSectionsId)
        {
            Query.Where(x => convocationSectionsId.Contains(x.ConvocationSectionId));
        }

        public ConvocationQuestionFilterSpecification(Guid convocationSectionId, string description, Guid? ignogredId = null)
        {
            Query.Where(x => x.ConvocationSectionId == convocationSectionId && x.Description.ToLower().Trim().Contains(description.ToLower().Trim()) && x.Id != ignogredId);
        }
    }
}
