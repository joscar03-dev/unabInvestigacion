using AKDEMIC.DOMAIN.Entities.General;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.ConferenceViewModels;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedBookViewModels
{
    public class PublishedBookCreateViewModel
    {
        public string MainAuthor { get; set; } //Autor principal
        public string Title { get; set; } //Titulo del libro
        public string PublishingCity { get; set; } //Ciudad de Edición
        public string PublishingHouse { get; set; } //Editorial
        public int PublishingYear { get; set; } //Año de Edición
        public int PagesCount { get; set; } //Número de Páginas
        public string ISBN { get; set; } //ISBN
        public string LegalDeposit { get; set; } //Depósito legal
        public string Url { get; set; }

        public bool TermnConditions { get; set; }

        public List<PublishedBookFileViewModel> PublishedBookFiles { get; set; }
        public List<PublishedBookAuthorViewModel> PublishedBookAuthors { get; set; }
    }

    public class PublishedBookFileViewModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class PublishedBookAuthorViewModel
    {
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }

    }
}
