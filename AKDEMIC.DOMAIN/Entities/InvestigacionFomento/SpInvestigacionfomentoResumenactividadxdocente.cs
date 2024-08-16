using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class SpInvestigacionfomentoResumenactividadxdocente : BaseEntity, ITimestamp, IAggregateRoot
    {

        public Guid Id { get; set; }

        public string nombre { get; set; }

        public string? nombreactividad { get; set; }
        public string? valormes1 { get; set; }
        public string? valormes2 { get; set; }
        public string? valormes3 { get; set; }
        public string? valormes4 { get; set; }
        public string? valormes5 { get; set; }
        public string? valormes6 { get; set; }
        public string? valormes7 { get; set; }
        public string? valormes8 { get; set; }
        public string? valormes9 { get; set; }
        public string? valormes10 { get; set; }
        public string? valormes11 { get; set; }
        public string? valormes12 { get; set; }

        public string? valormes13 { get; set; }
        public string? valormes14 { get; set; }
        public string? valormes15 { get; set; }
        public string? valormes16 { get; set; }

        public string? valormes17 { get; set; }
        public string? valormes18 { get; set; }
        public string? valormes19 { get; set; }
        public string? valormes20 { get; set; }

        public int cumplio { get; set; }
        public int nocumplio { get; set; }

        public int totalmes { get; set; }

        public string? observaciones { get; set; }
    }
}
