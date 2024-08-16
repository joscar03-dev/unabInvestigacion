using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaEstructurainvestigacionViewModels
{
    public class InvestigacionasesoriaEstructurainvestigacionEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdTipotrabajoinvestigacion { get; set; }
        public int codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
    }
}
