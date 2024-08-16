using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class InvestigacionasesoriaAsesoriaestructuraalumnoobservacion : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdAsesoriaestructuraalumno { get; set; }   
        public Guid IdEstructurarequisito { get; set; }
        public string observaciones { get; set; }
        public string estado { get; set; }
        public string vigente { get; set; }





    }
}
