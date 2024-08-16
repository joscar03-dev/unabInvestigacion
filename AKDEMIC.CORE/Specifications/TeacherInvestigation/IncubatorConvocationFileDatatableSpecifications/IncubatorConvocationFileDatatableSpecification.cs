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

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.IncubatorConvocationFileDatatableSpecifications
{
    public sealed class IncubatorConvocationFileDatatableSpecification : Specification<IncubatorConvocationFile, object>
    {
        public IncubatorConvocationFileDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Guid incubatorConvocationId, Expression<Func<IncubatorConvocationFile, dynamic>> orderByPredicate = null, string searchValue = null)
        {
            Query.Where(x => x.IncubatorConvocationId == incubatorConvocationId);

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
