using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.MaestroAlumnoViewModels
{
    public class MaestroAlumnoCreateViewModel
    {

        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdCarrera { get; set; }
        public Guid IdUser { get; set; }
        public string codigo { get; set; }
        public string activo { get; set; }


    }
}
