using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationVacancySpecification
{
    public sealed class ConvocationVacancyDatatableSpecification : Specification<ConvocationVacancy, object>
    {
        public ConvocationVacancyDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<ConvocationVacancy, dynamic>> orderByPredicate, Guid convocationId, Guid? academicDepartmentId)
        {
            Query.Where(x => x.ConvocationId == convocationId);

            if (academicDepartmentId.HasValue)
            {
                Query.Where(x => x.AcademicDepartmentId == academicDepartmentId);
            }

            Query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
            .Select(x => new
            {
                x.Id,
                x.Vacancies,
                x.AcademicDepartmentId,
                x.AcademicDepartmentText,
                x.ContractType,
                x.Category,
                x.Dedication,
                x.Subjects,
                x.Requirements,
                x.Code
            })
            .Skip(sentParameters.PagingFirstRecord)
            .Take(sentParameters.RecordsPerDraw);

        }
    }
}
