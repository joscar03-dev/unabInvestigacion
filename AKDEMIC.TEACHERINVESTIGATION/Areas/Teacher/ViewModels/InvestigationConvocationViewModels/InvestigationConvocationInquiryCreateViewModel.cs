using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationConvocationInquiryCreateViewModel
    {

        public string Inquiry { get; set; }

        public string FilePath { get; set; }

        public IFormFile File { get; set; }

        public Guid InvestigationConvocationPostulantId { get; set; }
    }

}
