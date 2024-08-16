using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoViewModels
{
    public class InvestigacionformativaPlantrabajoEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid IdDocente { get; set; }
        public Guid IdLinea { get; set; }
        public Guid IdDepartamento { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdAreaacademica { get; set; }
        public Guid IdTipoevento { get; set; }
        public Guid IdTiporesultado { get; set; }
        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public IFormFile File { get; set; }

        public string titulo { get; set; }
        public string objetivo { get; set; }
        public string descripcion { get; set; }
        public string resultado { get; set; }
        public string estado { get; set; }
        public string activo { get; set; }
        public string coautor1 { get; set; }
        public string coautor2 { get; set; }

        public string observacion { get; set; }

    }
}
