using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaTipoeventoViewModels
{
    public class InvestigacionformativaTipoeventoEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public string activo { get; set; }
    }
}
