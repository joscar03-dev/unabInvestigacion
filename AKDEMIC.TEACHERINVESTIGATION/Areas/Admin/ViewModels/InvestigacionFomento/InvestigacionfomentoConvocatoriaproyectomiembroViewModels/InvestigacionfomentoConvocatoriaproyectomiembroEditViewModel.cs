using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectomiembroViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectomiembroEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }


        public string apellidopaterno { get; set; }

        public string apellidomaterno { get; set; }
        public string nombres { get; set; }
        public string numerodocumento { get; set; }


    }
}
