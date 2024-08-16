using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorPostulationTeamMember : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Del api se trae la data de los estudiantes que pueden ser miembros
        public Guid Id { get; set; }
        public string UserName { get; set; }  //Informacion traida del Api
        public string PaternalSurname { get; set; }  //Informacion traida del Api
        public string MaternalSurname { get; set; }  //Informacion traida del Api
        public string Name { get; set; }  //Informacion traida del Api
        public string Dni { get; set; } //Informacion traida del Api
        public int Sex { get; set; }  //Informacion traida del Api
        public int CurrentAcademicYear { get; set; } //Informacion traida del Api
        public string CareerText { get; set; }  //Informacion traida del Api
        public Guid IncubatorPostulationId { get; set; }
        public IncubatorPostulation IncubatorPostulation { get; set; }

    }
}
