using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class Unit : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; } //Jefe de Unidad
        public ApplicationUser User { get; set; }
        public ICollection<OperativePlan> OperativePlans { get; set; }
    }
}
