using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherInvestigation.IncubatorConvocationAnnexDatatableSpecifications
{
    public sealed class IncubatorConvocationAnnexFilterSpecification : Specification<IncubatorConvocationAnnex, object>
    {
        public IncubatorConvocationAnnexFilterSpecification(Guid incubatorConvocationId, string searchValue = null)
        {
            Query.Where(x => x.IncubatorConvocationId == incubatorConvocationId);

            if (!string.IsNullOrEmpty(searchValue))
                Query.Where(x => x.Name.ToLower().Contains(searchValue.ToLower()));
        }
    }
}
