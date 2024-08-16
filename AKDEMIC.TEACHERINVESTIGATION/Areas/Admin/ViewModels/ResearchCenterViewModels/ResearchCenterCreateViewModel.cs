using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ResearchCenterViewModels
{
    public class ResearchCenterCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
