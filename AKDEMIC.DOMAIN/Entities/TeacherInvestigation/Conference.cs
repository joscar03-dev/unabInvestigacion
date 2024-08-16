using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class Conference : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Congreso
        public Guid Id { get; set; }
        public Guid? OpusTypeId { get; set; } //Tipo de Trabajo/obra
        public OpusType OpusType { get; set; }
        [Required]
        public string UserId { get; set; } //El congreso le pertenece a un usuario
        public ApplicationUser User { get; set; }
        public int Type { get; set; } //Tipo de congreso AKDEMIC.CORE.Constants.Systems.TeacherInvestigationConstants.Conference.Type
        public string Title { get; set; } //Titulo del congreso
        public string Name { get; set; } //Nombre del congreso
        public string OrganizerInstitution { get; set; } //Institución organizadora
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string MainAuthor { get; set; } //Autor principal
        public string ISBN { get; set; } //ISBN
        public string ISSN { get; set; } //ISSN
        public string DOI { get; set; } //Digital Object Identifier
        public string UrlEvent { get; set; } //URL del evento

        public ICollection<ConferenceAuthor> ConferenceAuthors { get; set; }
        public ICollection<ConferenceFile> ConferenceFiles { get; set; }
    }
}
