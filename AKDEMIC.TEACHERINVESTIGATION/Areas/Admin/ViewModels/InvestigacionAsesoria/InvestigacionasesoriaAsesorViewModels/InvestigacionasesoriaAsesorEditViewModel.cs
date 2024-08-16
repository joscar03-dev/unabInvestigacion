using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesorViewModels
{
    public class InvestigacionasesoriaAsesorEditViewModel
    {
        [Required]
        public Guid Id { get; set; }
        public Guid IdCarrera { get; set; }
        public Guid IdUser { get; set; }

    }
}
