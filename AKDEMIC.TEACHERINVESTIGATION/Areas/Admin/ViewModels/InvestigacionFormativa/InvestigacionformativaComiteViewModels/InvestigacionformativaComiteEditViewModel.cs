using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaComiteViewModels
{
    public class InvestigacionformativaComiteEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdFacultad { get; set; }
     
        public Guid IdUser { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
    }
}
