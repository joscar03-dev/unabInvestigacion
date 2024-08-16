using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationRubricSectionSpecification
{
    public sealed class ConvocationRubricSectionFilterSpecification : Specification<ConvocationRubricSection>
    {
        public ConvocationRubricSectionFilterSpecification(Guid convocationId, byte type)
        {
            Query.Where(x => x.ConvocationId == convocationId && x.Type == type);
        }
    }
}
