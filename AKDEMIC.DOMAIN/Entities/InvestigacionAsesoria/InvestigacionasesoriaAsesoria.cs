using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class InvestigacionasesoriaAsesoria : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdAsesor { get; set; }
        public Guid IdAlumno { get; set; }
        public Guid? IdAlumno2 { get; set; }
        public Guid IdAnio { get; set; }

        public Guid IdTipotrabajoinvestigacion { get; set; }
        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string nroresolucion { get; set; }       
        public string archivourlresolucion { get; set; }
        public string activo { get; set; }


    }
}
