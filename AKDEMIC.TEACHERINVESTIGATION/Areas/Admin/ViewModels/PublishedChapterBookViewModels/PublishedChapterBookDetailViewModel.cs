using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublishedChapterBookViewModels
{
    public class PublishedChapterBookDetailViewModel
    {
        public Guid Id { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
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
}
