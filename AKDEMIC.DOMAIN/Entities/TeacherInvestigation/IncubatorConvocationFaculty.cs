using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorConvocationFaculty : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IncubatorConvocationId { get; set; }
        public IncubatorConvocation IncubatorConvocation { get; set; }
        public Guid FacultyId { get; set; } //Informacion del Api
        public string FacultyText { get; set; }
    }
}
