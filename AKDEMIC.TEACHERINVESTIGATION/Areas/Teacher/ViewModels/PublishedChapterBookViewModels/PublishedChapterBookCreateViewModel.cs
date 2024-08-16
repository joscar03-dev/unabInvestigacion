using AKDEMIC.DOMAIN.Entities.General;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedChapterBookViewModels
{
    public class PublishedChapterBookCreateViewModel
    {
        public string MainAuthor { get; set; } //Autor principal
        public string BookTitle { get; set; } //Título del libro
        public string ChapterTitle { get; set; } //Título del capítulo
        public string PublishingCity { get; set; } //Ciudad de Edición
        public string PublishingHouse { get; set; } //Editorial
        public int PublishingYear { get; set; } //Año de Edición
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public string DOI { get; set; } //Digital Object Identifier
        public string ISBN { get; set; } //ISBN
        public string Url { get; set; }

        public bool TermnConditions { get; set; }

        public List<PublishedChapterBookFileViewModel> PublishedChapterBookFiles { get; set; }
        public List<PublishedChapterBookAuthorViewModel> PublishedChapterBookAuthors { get; set; }
    }

    public class PublishedChapterBookFileViewModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class PublishedChapterBookAuthorViewModel
    {
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }

    }
}
