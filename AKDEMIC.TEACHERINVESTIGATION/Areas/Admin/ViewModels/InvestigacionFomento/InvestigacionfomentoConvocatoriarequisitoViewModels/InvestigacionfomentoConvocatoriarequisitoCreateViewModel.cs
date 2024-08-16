using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriarequisitoViewModels
{
    public class InvestigacionfomentoConvocatoriarequisitoCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdArea { get; set; }
        public Guid IdConvocatoria { get; set; }

        public Guid IdRequisito { get; set; }
        public IFormFile File { get; set; }



    }
}
