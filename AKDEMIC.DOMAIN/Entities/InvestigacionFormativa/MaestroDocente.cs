using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class MaestroDocente : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdFacultad { get; set; }
        public Guid IdDepartamento { get; set; }
        public Guid IdCategoriadocente { get; set; }
        public Guid IdTipogrado { get; set; }
        public string codigo { get; set; }
        public string perfil { get; set; }
        public string activo { get; set; }

    }
}
