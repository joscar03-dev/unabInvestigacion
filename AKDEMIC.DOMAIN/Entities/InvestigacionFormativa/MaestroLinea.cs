using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class MaestroLinea : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string codigo { get; set; }
        public Guid IdAreaacademica { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
    
        public string activo { get; set; }

    }
}
