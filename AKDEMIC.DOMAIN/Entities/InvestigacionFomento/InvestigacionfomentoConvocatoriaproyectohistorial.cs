﻿using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class InvestigacionfomentoConvocatoriaproyectohistorial : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }
        
        public string estado { get; set; }
        public string observacion { get; set; }



    }
}
