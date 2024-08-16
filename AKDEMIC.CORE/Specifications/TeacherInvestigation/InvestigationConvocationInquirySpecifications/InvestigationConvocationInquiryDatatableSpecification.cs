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
    public sealed class InvestigationConvocationInquiryDatatableSpecification : Specification<InvestigationConvocationInquiry, object>
    {
        public InvestigationConvocationInquiryDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Guid investigationConvocationId, Expression<Func<InvestigationConvocationInquiry, dynamic>> orderByPredicate = null, string searchValue = null)
        {
            Query.Where(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationConvocationId);

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Inquiry.ToLower().Contains(searchValue.ToLower()));

            Query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    x.Inquiry,
                    x.InvestigationConvocationPostulant.User.UserName,
                    x.InvestigationConvocationPostulant.User.FullName,
                    x.FilePath
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
