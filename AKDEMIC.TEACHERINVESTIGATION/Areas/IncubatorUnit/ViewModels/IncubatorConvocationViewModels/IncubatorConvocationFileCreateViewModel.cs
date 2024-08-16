using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorConvocationFileCreateViewModel
    {
        public string Name { get; set; }
        public string Number { get; set; }
        public IFormFile File { get; set; }
    }
}
