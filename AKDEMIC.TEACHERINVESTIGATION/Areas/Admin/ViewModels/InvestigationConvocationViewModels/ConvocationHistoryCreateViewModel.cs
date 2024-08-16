using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationHistoryCreateViewModel
    {
        public Guid InvestigationConvocationId { get; set; }
        public string UserId { get; set; }
        public string OldEndDate { get; set; }
        public string NewEndDate { get; set; }
        public IFormFile File { get; set; }
        public string ResolutionUrl { get; set; }
    }
}
