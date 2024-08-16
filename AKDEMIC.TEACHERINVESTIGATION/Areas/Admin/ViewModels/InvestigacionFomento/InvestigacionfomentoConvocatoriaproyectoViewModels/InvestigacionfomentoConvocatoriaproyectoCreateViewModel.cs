using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdConvocatoria { get; set; }
        public Guid IdLinea { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdArea { get; set; }
        public string nombre { get; set; }

        public string objetivoprincipal { get; set; }

        public decimal presupuesto { get; set; }

        public string estado { get; set; }
        public Guid IdDocente { get; set; }
        public int nromeses { get; set; }

    }
}
