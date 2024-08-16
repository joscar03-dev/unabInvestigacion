using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationSpecifications
{
    public sealed class ConvocationDatatableSpecification : Specification<Convocation, object>
    {
        public ConvocationDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<Convocation, dynamic>> orderByPredicate = null, string searchValue = null, ClaimsPrincipal user = null) 
        {
            if(user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(user.IsInRole(GeneralConstants.ROLES.CONVOCATION_PRESIDENT) || user.IsInRole(GeneralConstants.ROLES.CONVOCATION_MEMBER))
                {
                    Query.Where(x => x.ConvocationComitees.Any(y => y.UserId == userId));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));

            Query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.MinScore,
                    StartDate = x.StartDate.ToDateFormat(),
                    EndDate = x.EndDate.ToDateFormat()
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
