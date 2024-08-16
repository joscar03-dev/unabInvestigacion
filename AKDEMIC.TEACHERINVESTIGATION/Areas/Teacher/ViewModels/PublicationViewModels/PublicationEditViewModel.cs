using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublicationViewModels
{
    public class PublicationEditViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Orden Autoria", Prompt = "Orden Autoria")]
        public Guid? AuthorShipOrderId { get; set; }

       
        [Display(Name = "Tipo de Trabajo", Prompt = "Tipo de Trabajo")]
        public Guid? OpusTypeId { get; set; }


        [Display(Name = "Función", Prompt = "Función")]
        public Guid? PublicationFunctionId { get; set; }


        [Display(Name = "Indexado en", Prompt = "Indexado en")]
        public Guid? IndexPlaceId { get; set; }


        [Display(Name = "Identificación", Prompt = "Identificación")]
        public Guid? IdentificationTypeId { get; set; }

   
        [Display(Name = "Categoría de Trabajo", Prompt = "Categoría de Trabajo")]
        public int WorkCategory { get; set; }

   
        [Display(Name = "Título", Prompt = "Título")]
        public string Title { get; set; }
        [Display(Name = "Sub Título", Prompt = "Sub Título")]
        public string SubTitle { get; set; }
        [Display(Name = "Revista", Prompt = "Revista")]
        public string Journal { get; set; } //Revista
        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }
        [Display(Name = "Volumen", Prompt = "Volumen")]
        public string Volume { get; set; } //Volumen
        [Display(Name = "Fascículo", Prompt = "Fascículo")]
        public string Fascicle { get; set; } //Fascículo
        [Display(Name = "Autor principal", Prompt = "Autor principal")]
        public string MainAuthor { get; set; } //Autor principal
        [Display(Name = "Editorial", Prompt = "Editorial")]
        public string PublishingHouse { get; set; } //Editorial
        public string DOI { get; set; } //Digital Object Identifier

       
        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string PublishDate { get; set; }
    }

    public class PublicationAuthorCreateViewModel
    {
        public Guid PublicationId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }

    }

    public class PublicationAuthorEditViewModel
    {
        public Guid Id { get; set; }
        public Guid PublicationId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }
    }

    public class PublicationFileCreateViewModel
    {
        public Guid PublicationId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }

    }

    public class PublicationFileEditViewModel
    {
        public Guid Id { get; set; }
        public Guid PublicationId { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
    }
}
