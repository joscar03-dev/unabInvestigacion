using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraalumnoViewModels
{
    public class InvestigacionasesoriaAsesoriaestructuraalumnoCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdAlumno { get; set; }
        public Guid IdAsesoriaestructura { get; set; }

        public string descripcion { get; set; }
        public string rutaarchivo { get; set; }
        public string nombrearchivo { get; set; }
        public string observacion { get; set; }
        public string estado { get; set; }



    }
}
