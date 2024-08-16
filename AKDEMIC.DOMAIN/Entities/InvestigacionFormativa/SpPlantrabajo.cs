using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionFormativa
{
    public class SpPlantrabajo : BaseEntity, ITimestamp, IAggregateRoot
    {

        public Guid Id { get; set; }

        public string nro { get; set; }
        public string codigoplantrabajo { get; set; }
        public string titulo { get; set; }
         public string nombredocente { get; set; }

        public string nombreareaacademica { get; set; }
        public string nombredepartamento { get; set; }
        public string nombretipoevento { get; set; }
        public string nombrefacultad { get; set; }
        public string titulolinea { get; set; }
        public string coautor1 { get; set; }
        public string coautor2 { get; set; }

        public string nombretiporesultado { get; set; }
        public string nombreestado { get; set; }
        public string estado { get; set; }

        public string act1 { get; set; }
        public string act2 { get; set; }
        public string act3 { get; set; }
            
        public string act4 { get; set; }
        public string act5 { get; set; }
        public string act6 { get; set; }
        public string act7 { get; set; }
        public string act8 { get; set; }
        public string act9 { get; set; }
        public string act10 { get; set; }
        public string act11 { get; set; }
        public string act12 { get; set; }
            
        public string act13 { get; set; }
        public string act14 { get; set; }
            
        public string act15 { get; set; }
        public string act16 { get; set; }
            
        public string act17 { get; set; }
        public string act18 { get; set; }
        public string act19 { get; set; }
        public string act20 { get; set; }

            



    }
}
