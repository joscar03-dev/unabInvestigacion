using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationVacancySpecification
{
    public sealed class ConvocationVacancyFilterSpecification : Specification<ConvocationVacancy>
    {
        public ConvocationVacancyFilterSpecification(Guid convocationId, Guid? academicDepartmentId) 
        {
            Query.Where(x => x.ConvocationId == convocationId);

            if (academicDepartmentId.HasValue)
            {
                Query.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }
        }
    }
}
