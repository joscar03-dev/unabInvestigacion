using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorConvocationFileEditViewModel
    {
        public Guid IncubatorConvocationFileId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public IFormFile File { get; set; }
    }
}
