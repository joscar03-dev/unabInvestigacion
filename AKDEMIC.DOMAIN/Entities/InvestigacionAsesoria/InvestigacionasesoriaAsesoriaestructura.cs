using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class InvestigacionasesoriaAsesoriaestructura : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid? IdEstructura { get; set; }
        public Guid IdAsesoria { get; set; }       

        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        


    }
}
