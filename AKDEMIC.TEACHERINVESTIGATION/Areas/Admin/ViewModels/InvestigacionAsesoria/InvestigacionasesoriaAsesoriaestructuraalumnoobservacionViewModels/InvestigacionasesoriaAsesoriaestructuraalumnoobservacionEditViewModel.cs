using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraalumnoViewModels
{
    public class InvestigacionasesoriaAsesoriaestructuraalumnoobservacionEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdAsesoriaestructuraalumno { get; set; }

     
        public string observacion { get; set; }
        public string estado { get; set; }


    }
}
