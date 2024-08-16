using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaEstructurainvestigacionViewModels
{
    public class InvestigacionasesoriaEstructurainvestigacionrequisitoEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid Idestructurainvestigacion { get; set; }
        public int orden { get; set; }
        public string descripcion { get; set; }
    }
}
