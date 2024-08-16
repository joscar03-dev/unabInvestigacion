using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectTypeViewModels
{
    public class InvestigationProjectTypeCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
