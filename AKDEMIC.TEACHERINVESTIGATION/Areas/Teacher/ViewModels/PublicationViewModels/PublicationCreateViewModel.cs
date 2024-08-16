using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.PublicationViewModels
{
    public class PublicationCreateViewModel
    {
        [Display(Name = "Orden Autoria", Prompt = "Orden Autoria")]
        public Guid? AuthorShipOrderId { get; set; }

        [Required]
        [Display(Name = "Tipo de Trabajo", Prompt = "Tipo de Trabajo")]
        public Guid? OpusTypeId { get; set; }


        [Display(Name = "Función", Prompt = "Función")]
        public Guid? PublicationFunctionId { get; set; }


        [Display(Name = "Indexado en", Prompt = "Indexado en")]
        public Guid? IndexPlaceId { get; set; }


        [Display(Name = "Identificación", Prompt = "Identificación")]
        public Guid? IdentificationTypeId { get; set; }

        [Required]
        [Display(Name = "Categoría de Trabajo", Prompt = "Categoría de Trabajo")]
        public int WorkCategory { get; set; }

        [Required]
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

        [Required]
        [Display(Name = "Fecha de Publicación", Prompt = "Fecha de Publicación")]
        public string PublishDate { get; set; }

        public bool TermnConditions { get; set; }

        public List<PublicationFileViewModel> PublicationFiles { get; set; }
        public List<AuthorViewModel> Authors { get; set; }
    }

    public class PublicationFileViewModel
    {
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }

    public class AuthorViewModel
    {
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Dni { get; set; }

    }
}
