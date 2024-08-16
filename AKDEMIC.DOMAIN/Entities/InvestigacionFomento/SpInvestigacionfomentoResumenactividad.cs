using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFomento
{
    public class SpInvestigacionfomentoResumenactividad : BaseEntity, ITimestamp, IAggregateRoot
    {

        public Guid Id { get; set; }

               

        public string nombre { get; set; }
        public string periodoejecucion { get; set; }
        public string periodomonitoreo { get; set; }
        public string responsable { get; set; }

        
        public decimal totalactividades { get; set; }
        public decimal totalterminados { get; set; }
         public decimal totalnoterminados { get; set; }

        public string totalterminadosporcentaje { get; set; }
        public string totalnoterminadosporcentaje { get; set; }    

            



    }
}
