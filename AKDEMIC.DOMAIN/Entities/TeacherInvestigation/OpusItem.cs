using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class OpusItem : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Tipo de obra
        public Guid Id { get; set; }
        public Guid IdList { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Sistema { get; set; }

    }

    public class OpusItemPublication : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Tipo de obra
        public Guid Id { get; set; }
        public Guid IdList { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Sistema { get; set; }
        public int Orden { get; set; }
        public string Valor { get; set; }


    }
}
