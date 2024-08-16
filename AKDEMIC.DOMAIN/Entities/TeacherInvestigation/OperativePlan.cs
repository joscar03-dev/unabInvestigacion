using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class OperativePlan : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Year { get; set; }
        public DateTime RegisterDate { get; set; }
        public int State { get; set; } //Solicitado, Aprobado , Observado
        public string FilePath { get; set; }
        public string Observation { get; set; }
        public Guid UnitId { get; set; }
        public Unit Unit { get; set; }
    }
}
