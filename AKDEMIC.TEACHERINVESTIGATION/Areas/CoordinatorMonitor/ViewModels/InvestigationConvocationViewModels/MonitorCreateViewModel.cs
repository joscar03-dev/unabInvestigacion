using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.CoordinatorMonitor.ViewModels.InvestigationConvocationViewModels
{
    public class MonitorCreateViewModel
    {
        [Required]
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }
    }
}
