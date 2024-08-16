using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorConvocation : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Convocatorias de incubadoras de emprendimiento
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public DateTime InscriptionStartDate { get; set; }

        public DateTime InscriptionEndDate { get; set; }
        public string PicturePath { get; set; } //Foto
        public string AddressedTo { get; set; } //Dirigido A
        public string Requirements { get; set; } //Requisitos

        public string DocumentPath { get; set; } //Documento 

        public int TotalWinners { get; set; } //Cantidad de Ganadores por Convocatoria
        public ICollection<IncubatorPostulation> IncubatorPostulations { get; set; }//Postulantes a la convocatoria
        public ICollection<IncubatorConvocationFile> IncubatorConvocationFiles { get; set; }//Bases de la convocatoria
        public ICollection<IncubatorConvocationAnnex> IncubatorConvocationAnnexes { get; set; }//Documentos Solicitados para la convocatoria
        public ICollection<IncubatorConvocationFaculty> IncubatorConvocationFaculties { get; set; } //Facultades a los que ira la convocatoria
        public ICollection<IncubatorConvocationEvaluator> IncubatorConvocationEvaluators { get; set; } //Evaluadores de la convocatoria de Incubadora
        public ICollection<IncubatorMonitor> IncubatorMonitors { get; set; }
        public ICollection<IncubatorCoordinatorMonitor> IncubatorCoordinatorMonitors { get; set; }
    }
}
