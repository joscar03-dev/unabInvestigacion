using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ConfigurationViewModels
{
    public class ConfigurationViewModel
    {
        [Display(Name = "Formato de Articulo Cientifico", Prompt = "Formato de Articulo Cientifico")]
        public string RulesArticleScientific { get; set; }

        public IFormFile File { get; set; }

        [Display(Name = "Restringir la postulación a una convocatoría vigente", Prompt = "Restringir la postulación a una convocatoría vigente")]
        public bool HasSinglePostulantRestriction { get; set; }
        [Display(Name = "Permitir la solicitud de registro externo", Prompt = "Permitir la solicitud de registro externo")]
        public bool AllowRegistrationRequest { get; set; }

        [Display(Name = "Publicaciones terminos y condiciones", Prompt = "Publicaciones terminos y condiciones")]
        public string PublicationTermsAndCondition { get; set; }

    }
}
