using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorPostulationAnnex : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string FilePath { get; set; }
        public Guid IncubatorPostulationId { get; set; } //Postulacion del usuario y convocatoria a la que postulo
        public IncubatorPostulation IncubatorPostulation { get; set; }
        public Guid IncubatorConvocationAnnexId { get; set; } //Annexo creado en la convocatoria que deberia subir
        public IncubatorConvocationAnnex IncubatorConvocationAnnex { get; set; }
    }
}
