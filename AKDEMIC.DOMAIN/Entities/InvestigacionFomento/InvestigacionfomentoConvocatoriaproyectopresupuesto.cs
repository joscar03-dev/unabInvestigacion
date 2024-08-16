using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class InvestigacionfomentoConvocatoriaproyectopresupuesto: BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }

        public Guid IdConcepto { get; set; }
        public Guid IdGastotipo { get; set; }
        public Guid IdUnidadmedida { get; set; }
        
        public string descripcion { get; set; }
        public decimal costounitario { get; set; }
        public decimal cantidad { get; set; }
        public decimal total { get; set; }

        

    }
}
