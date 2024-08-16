using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoactividadViewModels
{
    public class InvestigacionformativaPlantrabajoactividadCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdPlantrabajo { get; set; }

        public IFormFile File { get; set; }

        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }



    }
}
