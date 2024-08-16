using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class MaestroAreasusuario: BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdArea { get; set; }
     
        public Guid IdUser { get; set; }

        public string activo { get; set; }



    }
}
