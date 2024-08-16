using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.ConferenceViewModels
{
    public class ConferenceCreateViewModel
    {
        public Guid? OpusTypeId { get; set; } //Tipo de Trabajo/obra
        public int Type { get; set; } //Tipo de congreso AKDEMIC.CORE.Constants.Systems.TeacherInvestigationConstants.Conference.Type
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

        public bool TermnConditions { get; set; }

        public List<ConferenceFileViewModel> ConferenceFiles { get; set; }
        public List<ConferenceAuthorViewModel> ConferenceAuthors { get; set; }
    }

    public class ConferenceFileViewModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class ConferenceAuthorViewModel
    {
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }

    }
}
