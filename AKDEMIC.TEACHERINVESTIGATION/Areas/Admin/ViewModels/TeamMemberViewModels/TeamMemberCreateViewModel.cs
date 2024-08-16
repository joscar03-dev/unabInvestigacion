using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.TeamMemberViewModels
{
    public class TeamMemberCreateViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
