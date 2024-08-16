using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class InvestigacionfomentoConvocatoriaproyectoflujoindicador : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }
        public Guid IdListaverificacion { get; set; }
        public Guid IdIndicador { get; set; }

        public Guid IdArea { get; set; }
        public string valor { get; set; }

        public string puntaje { get; set; }
        public string observacion { get; set; }

    }
}
