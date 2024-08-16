using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationSpecifications
{
    public sealed class ConvocationFilterSpecification : Specification<Convocation, object>
    {
        public ConvocationFilterSpecification(string searchValue = null, ClaimsPrincipal user = null)
        {
            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(GeneralConstants.ROLES.CONVOCATION_PRESIDENT) || user.IsInRole(GeneralConstants.ROLES.CONVOCATION_MEMBER))
                {
                    Query.Where(x => x.ConvocationComitees.Any(y => y.UserId == userId));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
        }
    }
}
