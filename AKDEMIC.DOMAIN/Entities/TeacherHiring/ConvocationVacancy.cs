using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ConvocationVacancy : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public Guid AcademicDepartmentId { get; set; } //Información del api
        public string AcademicDepartmentText { get; set; }

        public Guid ConvocationId { get; set; }
        public Convocation Convocation { get; set; }

        public string ContractType { get; set; }
        public string Category { get; set; }
        public string Dedication { get; set; }
        public string Subjects { get; set; }
        public string Requirements { get; set; }
        public int Vacancies { get; set; }
        public ICollection<ApplicantTeacher> ApplicantTeachers { get; set; }
    }
}
