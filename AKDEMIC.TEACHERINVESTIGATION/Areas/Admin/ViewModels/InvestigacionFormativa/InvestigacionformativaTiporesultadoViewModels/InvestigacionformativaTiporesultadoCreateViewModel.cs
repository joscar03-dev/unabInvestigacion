using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaTiporesultadoViewModels
{
    public class InvestigacionformativaTiporesultadoCreateViewModel
    {

        public Guid Id { get; set; }

        public string nombre { get; set; }

        public string descripcion { get; set; }

        public string activo { get; set; }

    }
}
