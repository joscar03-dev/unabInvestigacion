using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels
{
    public class PostulantTechnicalFileCreateViewModel
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
    }
}
