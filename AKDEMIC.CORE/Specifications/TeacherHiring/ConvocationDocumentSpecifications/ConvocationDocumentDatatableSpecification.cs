using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationDocumentSpecifications
{
    public sealed class ConvocationDocumentDatatableSpecification : Specification<ConvocationDocument, object>
    {
        public ConvocationDocumentDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<ConvocationDocument, dynamic>> orderByPredicate = null, Guid? convocationId = null, byte? type = null)
        {
            if (convocationId.HasValue)
                Query.Where(x => x.ConvocationId == convocationId);

            if (type.HasValue)
                Query.Where(x => x.Type == type);

            Query.OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.CreatedAt,
                    x.Url
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
