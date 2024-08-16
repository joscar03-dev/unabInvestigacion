using System;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels
{
    public class ScientificArticleViewModel
    {
        public Guid? Id { get; set; }
        public Guid InvestigationProjectId { get; set; }
        public string Title { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }


    }
}
