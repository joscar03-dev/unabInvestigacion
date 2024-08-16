using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationSpecifications
{
    public sealed class ConvocationSelectSpecification : Specification<Convocation, Select2Structs.Result>
    {
        public ConvocationSelectSpecification(Select2Structs.RequestParameters requestParameters)
        {
            if (!string.IsNullOrEmpty(requestParameters.SearchTerm))
            {
                var search = requestParameters.SearchTerm.Trim().ToLower();
                Query.Where(x => x.Name.Trim().ToLower().Contains(search));
            }

            int currentPage = requestParameters.CurrentPage != 0 ? requestParameters.CurrentPage - 1 : 0;

            Query
            .Skip(currentPage * GeneralConstants.SELECT2.DEFAULT.PAGE_SIZE)
            .Take(GeneralConstants.SELECT2.DEFAULT.PAGE_SIZE);

            Query.Select(x => new Select2Structs.Result
            {
                Id = x.Id,
                Text = x.Name
            });
        }
    }
}
