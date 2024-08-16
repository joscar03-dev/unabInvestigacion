using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationFileSpecifications
{
    public sealed class InvestigationConvocationFileFilterSpecification : Specification<InvestigationConvocationFile, object>
    {
        public InvestigationConvocationFileFilterSpecification(Guid investigationConvocationId, string searchValue = null)
        {
            Query.Where(x => x.InvestigationConvocationId == investigationConvocationId);

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
        }
    }
}
