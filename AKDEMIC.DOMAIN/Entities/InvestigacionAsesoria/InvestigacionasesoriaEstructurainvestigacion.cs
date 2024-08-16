using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class InvestigacionasesoriaEstructurainvestigacion : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public int codigo { get; set; }
        public Guid IdTipotrabajoinvestigacion { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }

    }
}
