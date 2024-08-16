using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class InvestigacionfomentoConvocatoriaproyectoactividaddetalle : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyectoactividad { get; set; }
        public Guid IdConvocatoriaproyectocronograma { get; set; }      


    }
}

