using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels
{
    public class PostulantTechnicalFileEditViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
        public string FilePath { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
    }
}
