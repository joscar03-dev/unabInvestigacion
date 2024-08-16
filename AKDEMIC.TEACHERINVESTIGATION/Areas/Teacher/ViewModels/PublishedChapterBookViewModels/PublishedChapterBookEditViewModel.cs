using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedChapterBookViewModels
{
    public class PublishedChapterBookEditViewModel
    {
        public Guid Id { get; set; }
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
    }

    public class PublishedChapterBookFileCreateViewModel
    {
        public Guid PublishedChapterBookId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }

    }

    public class PublishedChapterBookFileEditViewModel : PublishedChapterBookFileCreateViewModel
    {
        public Guid Id { get; set; }
    }

    public class PublishedChapterBookAuthorCreateViewModel
    {
        public Guid PublishedChapterBookId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }
    }

    public class PublishedChapterBookAuthorEditViewModel : PublishedChapterBookAuthorCreateViewModel
    {
        public Guid Id { get; set; }
    }
}
