using AKDEMIC.DOMAIN.Entities.General;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.General.ApplicationUserSpecifications
{
    public sealed class ApplicationUserFilterSpecification : Specification<ApplicationUser>
    {
        public ApplicationUserFilterSpecification(string userName = null)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                Query.Where(x => x.UserName == userName);
            }
        }

        public ApplicationUserFilterSpecification(List<string> roles, string search)
        {
            if (roles != null && roles.Any())
                Query.Where(x => x.UserRoles.Any(y => roles.Contains(y.Role.Name)));

            if (!string.IsNullOrEmpty(search))
                Query.Where(x => x.FullName.Trim().ToLower().Contains(search.ToLower().Trim()));
        }
    }
}
