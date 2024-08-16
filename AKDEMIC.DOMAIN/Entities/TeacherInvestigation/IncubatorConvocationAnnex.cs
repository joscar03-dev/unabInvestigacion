using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorConvocationAnnex : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Documentos solicitados en una Convocatoria
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public Guid IncubatorConvocationId { get; set; }
        public IncubatorConvocation IncubatorConvocation { get; set; }
    }
}
