using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationSpecifications
{
    public sealed class InvestigationConvocationFilterSpecification : Specification<InvestigationConvocation, object>
    {
        public InvestigationConvocationFilterSpecification(string searchValue = null)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                Query.Where(x => x.Name.ToUpper().Contains(searchValue.ToUpper()));
            }
        }
    }
}
