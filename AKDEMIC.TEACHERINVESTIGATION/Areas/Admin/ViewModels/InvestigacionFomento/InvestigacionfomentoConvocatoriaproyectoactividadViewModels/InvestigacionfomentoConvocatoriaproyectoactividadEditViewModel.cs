using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividadViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoactividadEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }
     

        public string titulo { get; set; }

        public IFormFile File { get; set; }


        public string estado { get; set; }

      
    }
}
