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

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationFileSpecifications
{
    public sealed class InvestigationConvocationFileDatatableSpecification : Specification<InvestigationConvocationFile, object>
    {
        public InvestigationConvocationFileDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Guid investigationConvocationId, Expression<Func<InvestigationConvocationFile, dynamic>> orderByPredicate = null, string searchValue = null)
        {
            Query.Where(x => x.InvestigationConvocationId == investigationConvocationId);

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));

            Query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Number,
                    x.FilePath
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
