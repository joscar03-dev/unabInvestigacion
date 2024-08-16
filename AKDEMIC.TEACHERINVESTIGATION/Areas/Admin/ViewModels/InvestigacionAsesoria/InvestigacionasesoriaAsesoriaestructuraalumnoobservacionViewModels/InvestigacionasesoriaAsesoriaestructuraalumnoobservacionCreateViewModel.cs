using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraalumnoViewModels
{
    public class InvestigacionasesoriaAsesoriaestructuraalumnoobservacionCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdAsesoriaestructuraalumno { get; set; }


        public string observacion { get; set; }
        public string estado { get; set; }



    }
}
