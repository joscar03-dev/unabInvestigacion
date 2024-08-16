using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriarequisitoViewModels
{
    public class InvestigacionfomentoConvocatoriarequisitoEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid IdArea { get; set; }
        public Guid IdConvocatoria { get; set; }

        public Guid IdRequisito { get; set; }
        public IFormFile File { get; set; }

    }
}
