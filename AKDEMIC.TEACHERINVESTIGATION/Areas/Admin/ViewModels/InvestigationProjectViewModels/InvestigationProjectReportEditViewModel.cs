using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectReportEditViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ExpirationDate { get; set; }
        public Guid InvestigationProjectId { get; set; }
    }
}
