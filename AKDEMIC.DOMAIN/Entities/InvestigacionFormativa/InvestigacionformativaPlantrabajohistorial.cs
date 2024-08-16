﻿using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class InvestigacionformativaPlantrabajohistorial : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdPlantrabajo { get; set; }
     
        public string observacion { get; set; }
        
        public string estado { get; set; }

    }
}
