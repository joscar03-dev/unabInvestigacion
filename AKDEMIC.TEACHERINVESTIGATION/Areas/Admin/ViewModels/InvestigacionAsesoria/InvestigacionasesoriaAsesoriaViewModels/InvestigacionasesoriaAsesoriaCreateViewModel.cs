using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaViewModels
{
    public class InvestigacionasesoriaAsesoriaCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdAsesor { get; set; }
        public Guid IdAlumno { get; set; }
        public Guid IdAlumno2 { get; set; }
        public Guid IdAnio { get; set; }
        public Guid IdTipotrabajoinvestigacion { get; set; }

        public string fechaini { get; set; }
        public string fechafin { get; set; }
        public string nroresolucion { get; set; }

        public string archivourlresolucion { get; set; }


        public string activo { get; set; }


    }
}
