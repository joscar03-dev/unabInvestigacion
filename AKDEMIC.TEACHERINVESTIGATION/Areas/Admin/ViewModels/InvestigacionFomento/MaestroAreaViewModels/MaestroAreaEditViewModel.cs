using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.MaestroAreaViewModels
{
    public class MaestroAreaEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdOficina { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
    }
}
