using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraViewModels
{
    public class InvestigacionasesoriaAsesoriaestructuraEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdEstructura { get; set; }
        public Guid IdAsesoria { get; set; }


        public string fechaini { get; set; }
        public string fechafin { get; set; }

    }
}
