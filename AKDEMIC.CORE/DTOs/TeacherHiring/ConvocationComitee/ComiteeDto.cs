using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationComitee
{
    public class ComiteeDto
    {
        public string UserFullName { get; set; }
        public string UserId { get; set; }
        public Guid ConvocationId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string AcademicDepartmentText { get; set; }
        public Guid AcademicDepartmentId { get; set; }
    }
}
