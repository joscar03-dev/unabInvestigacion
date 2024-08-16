using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionLaboratorio.InvestigacionlaboratorioHorarioViewModels
{
    public class InvestigacionlaboratorioHorarioCreateViewModel
    {
      
        [Required]
        public Guid Id { get; set; }
        public Guid IdLaboratorio { get; set; }
        public Guid IdDocente { get; set; }
        public Guid IdProyecto { get; set; }
        public Guid IdEquipo { get; set; }
        public string codigo { get; set; }
        public string actividad { get; set; }   
        public string activo { get; set; }
        public DateTime fechaini { get; set; }
        public DateTime fechafin { get; set; }



    }
}
