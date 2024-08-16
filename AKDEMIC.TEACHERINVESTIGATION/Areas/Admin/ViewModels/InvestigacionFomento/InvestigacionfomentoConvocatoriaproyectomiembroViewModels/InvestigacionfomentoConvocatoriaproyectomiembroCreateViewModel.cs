using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectomiembroViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectomiembroCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }


        public string apellidopaterno { get; set; }

        public string apellidomaterno { get; set; }
        public string nombres { get; set; }
        public string numerodocumento { get; set; }

    }

}
