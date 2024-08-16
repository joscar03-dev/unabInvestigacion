using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherSpecifications
{
    public sealed class ApplicantTeacherFilterSpecification : Specification<ApplicantTeacher, object>
    {
        public ApplicantTeacherFilterSpecification(Guid? convocationId, string search, ClaimsPrincipal user, Guid? convocationVacancyId)
        {
            if (user != null)
            {
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (user.IsInRole(GeneralConstants.ROLES.APPLICANT_TEACHER))
                {
                    Query.Where(x => x.UserId == userId);
                }
            }

            if (convocationId.HasValue)
                Query.Where(x => x.ConvocationId == convocationId);

            if (convocationVacancyId.HasValue)
                Query.Where(x => x.ConvocationVacancyId == convocationVacancyId);

            if (!string.IsNullOrEmpty(search))
                Query.Where(x => x.User.FullName.ToLower().Contains(search.ToLower()) || x.User.Dni.ToLower().Contains(search.ToLower()));
        }
    }
}
