using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationMonitorCreateViewModel
    {
        [Required]
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }

    }
}
