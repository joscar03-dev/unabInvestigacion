﻿using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class InvestigacionformativaConfiguracionanio : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
     
        public string nombre { get; set; }
 
        public string descripcion { get; set; }
        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string activo { get; set; }

    }
}
