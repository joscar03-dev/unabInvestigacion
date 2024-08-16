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
    public sealed class IncubatorConvocationFileFilterSpecification : Specification<IncubatorConvocationFile, object>
    {
        public IncubatorConvocationFileFilterSpecification(Guid incubatorConvocationId, string searchValue = null)
        {
            Query.Where(x => x.IncubatorConvocationId == incubatorConvocationId);

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
        }
    }
}
