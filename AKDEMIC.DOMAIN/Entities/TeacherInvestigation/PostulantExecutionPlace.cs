using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PostulantExecutionPlace : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid? DepartmentId { get; set; } //Informacion del Api
        public string DepartmentText { get; set; }

        public Guid? ProvinceId { get; set; } //Informacion del Api
        public string ProvinceText { get; set; }

        public Guid? DistrictId { get; set; } //Informacion del Api
        public string DistrictText { get; set; }

        public Guid InvestigationConvocationPostulantId { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }
    }
}
