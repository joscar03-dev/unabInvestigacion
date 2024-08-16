using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraViewModels
{
    public class InvestigacionasesoriaAsesoriaestructuraCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdEstructura { get; set; }
        public Guid IdAsesoria { get; set; }
       

        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
       


    }
}
