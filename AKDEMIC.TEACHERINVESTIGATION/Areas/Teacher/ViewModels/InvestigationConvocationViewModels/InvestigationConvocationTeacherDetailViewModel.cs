using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationConvocationTeacherDetailViewModel
    {
        public IFormFile File { get; set; }
        public string Inquiry { get; set; }
        public string FilePath { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
    }
}
