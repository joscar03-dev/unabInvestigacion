using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class InvestigacionformativaPlantrabajoactividad : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdPlantrabajo { get; set; }
     
        public string titulo { get; set; }
        public string descripcion { get; set; }

        public string observacion { get; set; }
        public string informefinal { get; set; }

        public string estado { get; set; }
        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string archivourl { get; set; }
        public string archivoobservacionurl { get; set; }
        public string anexourl { get; set; }






    }
}
