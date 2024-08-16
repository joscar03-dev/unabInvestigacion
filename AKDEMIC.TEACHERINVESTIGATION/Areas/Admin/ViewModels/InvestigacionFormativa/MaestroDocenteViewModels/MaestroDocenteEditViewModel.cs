using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroDocenteViewModels
{
    public class MaestroDocenteEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdFacultad { get; set; }
        public Guid IdDepartamento { get; set; }
        public Guid IdCategoriadocente { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdTipogrado { get; set; }       
        public string perfil { get; set; }
        public string activo { get; set; }
    }
}
