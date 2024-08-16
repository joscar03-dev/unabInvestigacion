using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class OpusTypeitem : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Tipo de obra
        public Guid Id { get; set; }
        public Guid IdType { get; set; }
        public Guid IdItem { get; set; }

        public int Orden { get; set; }
        public string activo { get; set; }

    }
}
