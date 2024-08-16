using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class InvestigacionasesoriaEstructurainvestigacionequisito : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid Idestructurainvestigacion { get; set; }
        public string descripcion { get; set; }
        public int orden { get; set; }

    }
}
