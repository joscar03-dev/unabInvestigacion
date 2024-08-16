using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoactividadViewModels
{
    public class InvestigacionformativaPlantrabajoactividadEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid IdPlantrabajoactividad { get; set; }
       
        public IFormFile File { get; set; }
        public IFormFile Fileanexo { get; set; }

        public string titulo { get; set; }
        public string descripcion { get; set; }

        public string observacion { get; set; }
        public string estado { get; set; }
       

    }
}
