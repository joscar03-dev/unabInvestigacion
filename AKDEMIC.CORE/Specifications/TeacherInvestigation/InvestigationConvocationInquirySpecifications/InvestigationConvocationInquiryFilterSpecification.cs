using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationInquirySpecifications
{
    public sealed class InvestigationConvocationInquiryFilterSpecification : Specification<InvestigationConvocationInquiry, object>
    {
        public InvestigationConvocationInquiryFilterSpecification(Guid investigationConvocationId, string searchValue = null)
        {
            Query.Where(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationConvocationId);

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Inquiry.ToLower().Contains(searchValue.ToLower()));
        }
    }
}
