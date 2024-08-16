using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoViewModels
{
    public class InvestigacionformativaPlantrabajoCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdDocente { get; set; }
        public Guid IdLinea { get; set; }
        public Guid IdDepartamento { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdAreaacademica { get; set; }
        public Guid IdTipoevento { get; set; }
        public Guid IdTiporesultado { get; set; }
        public Guid IdAnio { get; set; }
        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string titulo { get; set; }
        public string objetivo { get; set; }
        public string descripcion { get; set; }
        public string estado { get; set; }
        public string activo { get; set; }


    }
}
