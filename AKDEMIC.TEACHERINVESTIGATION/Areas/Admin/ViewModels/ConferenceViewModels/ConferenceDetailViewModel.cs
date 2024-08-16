using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ConferenceViewModels
{
    public class ConferenceDetailViewModel
    {
        public Guid Id { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string OpusTypeName { get; set; } //Tipo de Trabajo/obra
        public string TypeName { get; set; } //Tipo de congreso AKDEMIC.CORE.Constants.Systems.TeacherInvestigationConstants.Conference.Type
        public string Title { get; set; } //Titulo del congreso
        public string Name { get; set; } //Nombre del congreso
        public string OrganizerInstitution { get; set; } //Institución organizadora
        public string Country { get; set; }
        public string City { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string MainAuthor { get; set; } //Autor principal
        public string ISBN { get; set; } //ISBN
        public string ISSN { get; set; } //ISSN
        public string DOI { get; set; } //Digital Object Identifier
        public string UrlEvent { get; set; } //URL del evento
    }
}
