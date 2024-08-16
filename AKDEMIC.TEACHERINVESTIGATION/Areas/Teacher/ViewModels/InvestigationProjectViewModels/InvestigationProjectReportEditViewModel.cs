using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectReportEditViewModel
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
        public string ReportUrl { get; set; }
    }
}
