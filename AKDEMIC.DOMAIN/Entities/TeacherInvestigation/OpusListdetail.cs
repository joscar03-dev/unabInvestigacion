﻿using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class OpusListdetail : BaseEntity, ITimestamp, IAggregateRoot
    {
  //detalle de listas
        public Guid Id { get; set; }
        public Guid IdList { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    
    }
}
