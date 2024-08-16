using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorConvocationMonitorCreateViewModel
    {
        [Required]
        public string UserId { get; set; }
        public Guid IncubatorConvocationId { get; set; }
    }
}
