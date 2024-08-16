using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroLineaViewModels
{
    public class MaestroLineaEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdAreaacademica { get; set; }
        public string codigo { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
    }
}
