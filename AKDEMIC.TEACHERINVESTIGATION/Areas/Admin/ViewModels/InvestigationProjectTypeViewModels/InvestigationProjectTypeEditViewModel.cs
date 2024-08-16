using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectTypeViewModels
{
    public class InvestigationProjectTypeEditViewModel
    {
        public Guid Id { get; set; }
        [Required]

        public string Name { get; set; }
    }
}
