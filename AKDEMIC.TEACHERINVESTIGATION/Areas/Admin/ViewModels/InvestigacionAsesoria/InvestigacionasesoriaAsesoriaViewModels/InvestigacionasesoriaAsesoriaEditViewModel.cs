using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaViewModels
{
    public class InvestigacionasesoriaAsesoriaEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdAsesor { get; set; }
        public Guid IdAlumno { get; set; }
        public Guid IdAlumno2 { get; set; }
        public Guid IdAnio { get; set; }

        public Guid IdTipotrabajoinvestigacion { get; set; }

        public DateTime? fechaini { get; set; }
        public DateTime? fechafin { get; set; }
        public string nroresolucion { get; set; }

        public string archivourlresolucion { get; set; }


        public string activo { get; set; }

    }
}
