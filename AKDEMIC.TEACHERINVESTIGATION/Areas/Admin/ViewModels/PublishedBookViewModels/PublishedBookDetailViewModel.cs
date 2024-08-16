using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublishedBookViewModels
{
    public class PublishedBookDetailViewModel
    {
        public Guid Id { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }
        public string MainAuthor { get; set; } //Autor principal
        public string Title { get; set; } //Titulo del libro
        public string PublishingCity { get; set; } //Ciudad de Edición
        public string PublishingHouse { get; set; } //Editorial
        public int PublishingYear { get; set; } //Año de Edición
        public int PagesCount { get; set; } //Número de Páginas
        public string ISBN { get; set; } //ISBN
        public string LegalDeposit { get; set; } //Depósito legal
        public string Url { get; set; }
    }
}
