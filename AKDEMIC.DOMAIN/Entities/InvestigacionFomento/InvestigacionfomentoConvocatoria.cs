using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class InvestigacionfomentoConvocatoria : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdOficina { get; set; }

        public Guid IdCategoriaconvocatoria { get; set; }
        public Guid IdTipoconvocatoria { get; set; }
        public Guid IdFlujo { get; set; }
        public Guid IdFacultad { get; set; }

        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string nombre { get; set; }
       
        public string descripcion { get; set; }
        public string imagenarchivo { get; set; }
        public string dirigidoa { get; set; }
        public decimal presupuesto { get; set; }
        public DateTime? fechainiinscripcion { get; set; }
        public DateTime? fechafininscripcion { get; set; }
        public int nroplaza { get; set; }
        public string activo { get; set; }


    }
}
