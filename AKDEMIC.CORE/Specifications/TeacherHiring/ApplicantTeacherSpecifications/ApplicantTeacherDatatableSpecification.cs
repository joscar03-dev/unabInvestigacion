using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherSpecifications
{
    public sealed class ApplicantTeacherDatatableSpecification : Specification<ApplicantTeacher, object>
    {
        public ApplicantTeacherDatatableSpecification(DataTablesStructs.SentParameters sentParameters, Expression<Func<ApplicantTeacher, dynamic>> orderByPredicate, Guid? convocationId, string search, ClaimsPrincipal user = null)
        {
            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(GeneralConstants.ROLES.APPLICANT_TEACHER))
                {
                    Query.Where(x => x.UserId == userId);
                }

                if (user.IsInRole(GeneralConstants.ROLES.CONVOCATION_PRESIDENT) || user.IsInRole(GeneralConstants.ROLES.CONVOCATION_MEMBER))
                {
                    Query.Where(x => x.Convocation.ConvocationComitees.Any(y=>y.UserId == userId));
                }
            }

            if (convocationId.HasValue)
                Query.Where(x => x.ConvocationId == convocationId);

            if (!string.IsNullOrEmpty(search))
                Query.Where(x => x.User.FullName.ToLower().Contains(search.ToLower().Trim()) || x.User.Dni.ToLower().Contains(search.ToLower().Trim()));

            Query.OrderByCondition(sentParameters.OrderDirection, orderByPredicate);

            Query
            .Select(x => new
            {
                x.Id,
                x.UserId,
                x.User.FullName,
                x.User.Dni,
                x.ConvocationId,
                convocation = x.Convocation.Name,
                x.Convocation.EnabledMasterClass,
                createdAt = x.CreatedAt.ToLocalDateTimeFormat(),
                academicDepartment = x.ConvocationVacancy.AcademicDepartmentText,
                x.Valid,
                status = TeacherHiringConstants.ApplicantTeacher.Status.VALUES[x.Status],
            })
            .Skip(sentParameters.PagingFirstRecord)
            .Take(sentParameters.RecordsPerDraw);
        }
    }
}
