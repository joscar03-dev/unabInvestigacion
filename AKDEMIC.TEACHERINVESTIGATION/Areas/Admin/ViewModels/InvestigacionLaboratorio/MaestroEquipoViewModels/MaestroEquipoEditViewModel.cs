using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionLaboratorio.MaestroEquipoViewModels
{
    public class MaestroEquipoEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
     
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
    }
}
