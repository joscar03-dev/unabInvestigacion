using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationSectionSpecifications
{
    public sealed class ConvocationSectionFilterSpecification : Specification<ConvocationSection>
    {
        public ConvocationSectionFilterSpecification(string title, Guid convocationId ,Guid? ignoredId = null)
        {
            Query.Where(x => x.ConvocationId == convocationId);

            Query.Where(x => x.Title.ToLower().Trim().Contains(title.ToLower().Trim()));

            if (ignoredId.HasValue)
                Query.Where(x => x.Id != ignoredId);
        }

        public ConvocationSectionFilterSpecification(Guid convocationId)
        {
            Query.Where(x => x.ConvocationId == convocationId);
        }
    }
}
