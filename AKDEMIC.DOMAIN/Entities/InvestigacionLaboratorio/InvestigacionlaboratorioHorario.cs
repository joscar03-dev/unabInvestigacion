using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionLaboratorio
{
    public class InvestigacionlaboratorioHorario : BaseEntity, ITimestamp, IAggregateRoot
    {
        [Required]
        public Guid Id { get; set; }
        public Guid IdLaboratorio { get; set; }
        public Guid IdDocente { get; set; }
        public Guid IdProyecto { get; set; }
        public Guid IdEquipo { get; set; }
        public string codigo { get; set; }
        public string actividad { get; set; }
        public string activo { get; set; }
        public DateTime fechaini { get; set; }
        public DateTime fechafin { get; set; }

    }
}
