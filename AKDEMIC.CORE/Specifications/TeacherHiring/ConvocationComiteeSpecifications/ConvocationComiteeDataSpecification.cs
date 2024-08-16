using AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationComitee;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationComiteeSpecifications
{
    public sealed class ConvocationComiteeDataSpecification : Specification<ConvocationComitee, ComiteeDto>
    {
        public ConvocationComiteeDataSpecification(Guid convocationId)
        {
            Query.Where(x => x.ConvocationId == convocationId);

            Query
                .Select(x => new ComiteeDto
                {
                    ConvocationId = x.ConvocationId,
                    UserFullName = x.User.FullName,
                    UserId = x.UserId,
                    RoleId = x.ApplicationRoleId,
                    RoleName = x.ApplicationRole.Name,
                    AcademicDepartmentText = x.AcademicDepartmentText,
                    AcademicDepartmentId = x.AcademicDeparmentId
                });
        }
    }
}
