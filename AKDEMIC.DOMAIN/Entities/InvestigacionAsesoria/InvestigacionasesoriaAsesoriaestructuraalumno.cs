using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria
{
    public class InvestigacionasesoriaAsesoriaestructuraalumno : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid IdAlumno { get; set; }
        public Guid IdAsesoriaestructura { get; set; }       

        public string descripcion { get; set; }
        public string rutaarchivo { get; set; }
        public string nombrearchivo { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }




    }
}
