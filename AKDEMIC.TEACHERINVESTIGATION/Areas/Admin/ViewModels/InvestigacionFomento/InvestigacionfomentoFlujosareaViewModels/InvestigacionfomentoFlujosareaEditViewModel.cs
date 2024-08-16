using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoFlujosareaViewModels
{
    public class InvestigacionfomentoFlujosareaEditViewModel
    {

        [Required]
        public Guid Id { get; set; }
        [Required]

        public Guid IdFlujo { get; set; }
        public Guid IdArea { get; set; }
        public int orden { get; set; }
        public string activo { get; set; }
        public string retornadocente { get; set; }
    }
}
