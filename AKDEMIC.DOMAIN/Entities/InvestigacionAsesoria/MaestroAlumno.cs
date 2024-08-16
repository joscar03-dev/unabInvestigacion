using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class MaestroAlumno : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdCarrera { get; set; }
        public String codigo { get; set; }
        public String activo { get; set; }

    }
}
