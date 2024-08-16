using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationFileViewModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public IFormFile File { get; set; }
    }

    public class ConvocationFileEditViewModel: ConvocationFileViewModel
    {
        public Guid InvestigationConvocationFileId { get; set; }
    }
}
