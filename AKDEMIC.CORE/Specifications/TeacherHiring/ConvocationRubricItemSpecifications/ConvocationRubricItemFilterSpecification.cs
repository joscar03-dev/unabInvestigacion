using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationRubricItemSpecifications
{
    public class ConvocationRubricItemFilterSpecification : Specification<ConvocationRubricItem>
    {
        public ConvocationRubricItemFilterSpecification(Guid convocationRubricSectionId)
        {
            Query.Where(x => x.ConvocationRubricSectionId == convocationRubricSectionId);
        }

        public ConvocationRubricItemFilterSpecification(List<Guid> convocationRubricSectionsId)
        {
            Query.Where(x => convocationRubricSectionsId.Contains(x.ConvocationRubricSectionId));
        }
    }
}
