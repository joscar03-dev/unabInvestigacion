using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class ConferenceAuthor : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Autores secundarios por asi decirlo
        public Guid Id { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }
        public Guid ConferenceId { get; set; }
        public Conference Conference { get; set; }
    }
}
