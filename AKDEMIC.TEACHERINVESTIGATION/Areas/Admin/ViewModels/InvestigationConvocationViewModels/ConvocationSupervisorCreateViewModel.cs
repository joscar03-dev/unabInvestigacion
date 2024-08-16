using System;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationSupervisorCreateViewModel
    {
        [Required]
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }

    }
}
