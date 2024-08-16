using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectorequisitoViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectorequisitoCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }
        public Guid IdRequisito { get; set; }
        
        public IFormFile File { get; set; }
       
      

    }

    public class InvestigacionfomentoConvocatoriaproyectorequisitoMostrar
    {

       
        public Guid idRequisito { get; set; }



    }
    public class InvestigacionfomentoConvocatoriaproyectorequisitoMostrar2
    {


        public Guid idRequisito { get; set; }



    }
}
