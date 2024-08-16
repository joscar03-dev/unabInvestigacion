using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublishedBookViewModels
{
    public class PublishedBookEditViewModel
    {
        public Guid Id { get; set; }
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


    public class PublishedBookFileCreateViewModel
    {
        public Guid PublishedBookId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }

    }

    public class PublishedBookFileEditViewModel : PublishedBookFileCreateViewModel
    {
        public Guid Id { get; set; }
    }

    public class PublishedBookAuthorCreateViewModel
    {
        public Guid PublishedBookId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }
    }

    public class PublishedBookAuthorEditViewModel : PublishedBookAuthorCreateViewModel
    {
        public Guid Id { get; set; }
    }
}
