using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationCalendarSpecifications
{
    public sealed class ConvocationCalendarFilterSpecification : Specification<ConvocationCalendar, object>
    {
        public ConvocationCalendarFilterSpecification(Guid convocationId)
        {
            Query.Where(x => x.ConvocationId == convocationId);
        }
    }
}
