using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationDocumentSpecifications
{
    public sealed class ConvocationDocumentFilterSpecification : Specification<ConvocationDocument>
    {
        public ConvocationDocumentFilterSpecification(Guid convocationId, byte? type)
        {
            Query.Where(x => x.ConvocationId == convocationId);

            if (type.HasValue)
                Query.Where(x => x.Type == type);
        }
    }
}
