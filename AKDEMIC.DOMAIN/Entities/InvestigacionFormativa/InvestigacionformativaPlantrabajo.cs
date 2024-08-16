using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class InvestigacionformativaPlantrabajo : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdDocente { get; set; }
        public Guid IdLinea { get; set; }
        public Guid IdDepartamento { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdAreaacademica { get; set; }
        public Guid IdTipoevento { get; set; }
        public Guid IdTiporesultado { get; set; }
        public Guid IdAnio { get; set; }
        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string titulo { get; set; }
        public string objetivo { get; set; }
        public string descripcion { get; set; }

        public string observacioncomite { get; set; }
        public string observacionacompaniante { get; set; }
        public string resultado { get; set; }
        public string archivourl { get; set; }

        public string estado { get; set; }
        public string activo { get; set; }

        public string coautor1 { get; set; }
        public string coautor2 { get; set; }

    }
}
