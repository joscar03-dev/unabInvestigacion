using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroCarreraViewModels
{
    public class MaestroCarreraCreateViewModel
    {
      
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdFacultad { get; set; }
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }


    }
}
