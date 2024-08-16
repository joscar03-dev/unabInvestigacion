using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividaddetalleViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoactividaddetalleCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyectoactividad { get; set; }

        public Guid IdConvocatoriaproyectocronograma { get; set; }


    }

}
