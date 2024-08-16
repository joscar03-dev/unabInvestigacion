using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionLaboratorio.MaestroLaboratorioViewModels
{
    public class MaestroLaboratorioEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
     
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string activo { get; set; }
    }
}
