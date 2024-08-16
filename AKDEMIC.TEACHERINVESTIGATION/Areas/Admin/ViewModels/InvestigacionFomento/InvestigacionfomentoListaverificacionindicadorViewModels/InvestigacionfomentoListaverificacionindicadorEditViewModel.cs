using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoListaverificacionindicadorViewModels
{
    public class InvestigacionfomentoListaverificacionindicadorEditViewModel
    {

        [Required]
        public Guid Id { get; set; }
        [Required]

        public Guid IdListaverificacion { get; set; }
        public Guid IdIndicador { get; set; }
        public string activo { get; set; }
    }
}
