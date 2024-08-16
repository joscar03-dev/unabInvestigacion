using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationConvocationInquiryEditViewModel
    {
        public Guid InvestigationConvocationFileInquiryId { get; set; }

        public string Inquiry { get; set; }
        public IFormFile File { get; set; }
        public string FilePath { get; set; }

        public Guid InvestigationConvocationPostulantId { get; set; }
    }
}
