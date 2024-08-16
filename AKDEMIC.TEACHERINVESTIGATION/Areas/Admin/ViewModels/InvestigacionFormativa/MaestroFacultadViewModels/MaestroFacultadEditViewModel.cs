using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroFacultadViewModels
{
    public class MaestroFacultadEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
     
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
    }
}
