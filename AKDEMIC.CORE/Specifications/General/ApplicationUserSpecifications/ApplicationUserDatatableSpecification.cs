using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.General.ApplicationUserSpecifications
{
    public sealed class ApplicationUserDatatableSpecification : Specification<ApplicationUser, object>
    {
        public ApplicationUserDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null, List<string> roles = null, string search = null)
        {
            if (roles != null && roles.Any())
                Query.Where(x => x.UserRoles.Any(y => roles.Contains(y.Role.Name)));

            if (!string.IsNullOrEmpty(search))
                Query.Where(x => x.FullName.Trim().ToLower().Contains(search.ToLower().Trim()));

            Query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.UserName,
                    x.Dni,
                    x.Email
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw);
        }
    }
}
