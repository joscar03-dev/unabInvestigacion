using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using Ardalis.Specification;
using System.Collections.Generic;
using System.Linq;

namespace AKDEMIC.CORE.Specifications.General.ApplicationUserSpecifications
{
    public sealed class ApplicationUserSelectSpecification : Specification<ApplicationUser, Select2Structs.Result>
    {
        public ApplicationUserSelectSpecification(Select2Structs.RequestParameters requestParameters, List<string> ignoredRoles = null, List<string> selectedRoles = null)
        {
            if (ignoredRoles != null && ignoredRoles.Any())
                Query.Where(x => !x.UserRoles.Any(ur => ignoredRoles.Contains(ur.Role.Name)));

            if (selectedRoles != null && selectedRoles.Any())
                Query.Where(x => x.UserRoles.Any(ur => selectedRoles.Contains(ur.Role.Name)));

            if (!string.IsNullOrEmpty(requestParameters.SearchTerm))
            {
                var search = requestParameters.SearchTerm.Trim().ToLower();
                Query.Where(x => x.FullName.ToLower().Contains(search) || x.UserName.ToLower().Contains(search));
            }

            int currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;

            Query
            .Skip(currentPage * GeneralConstants.SELECT2.DEFAULT.PAGE_SIZE)
            .Take(GeneralConstants.SELECT2.DEFAULT.PAGE_SIZE);

            Query.Select(x => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.FullName
            });
        }
    }
}
