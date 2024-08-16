using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ResearchLineViewModels
{
    public class ResearchLineEditViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid ResearchLineCategoryId { get; set; }
    }
}
