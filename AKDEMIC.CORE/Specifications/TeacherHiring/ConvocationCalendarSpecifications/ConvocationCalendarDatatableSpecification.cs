using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationCalendarSpecifications
{
    public sealed class ConvocationCalendarDatatableSpecification : Specification<ConvocationCalendar, object>
    {
        public ConvocationCalendarDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<ConvocationCalendar, dynamic>> orderByPredicate = null, Guid? convocationId = null)
        {
            Query.Where(x => x.ConvocationId == convocationId);

            Query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    StartDate = x.StartDate.ToDateFormat(),
                    EndDate = x.EndDate.ToDateFormat(),
                    x.Description
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
