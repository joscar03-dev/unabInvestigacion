using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class MaestroAnio : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public String anio { get; set; }

    }
}
