using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividaddetalleViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoactividaddetalleEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyectoactividad { get; set; }

        public Guid IdConvocatoriaproyectocronograma { get; set; }
       

      
    }
}
