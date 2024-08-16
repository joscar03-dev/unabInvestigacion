using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectViewModel
    {
        public Guid InvestigationProjectId { get; set; }
        public Guid? InvestigationProjectTypeId { get; set; }
        public string GanttDiagramUrl { get; set; }
        public IFormFile GanttDiagramFile { get; set; }
        public string ExecutionAddress { get; set; }
        public string FinalReportUrl { get; set; }
        public IFormFile FinalReportFile { get; set; }
        public string ProjectTitle { get; set; }
        public string GeneralGoal { get; set; }
        public string SpecificGoal { get; set; }
        public string FinancingInvestigation { get; set; }
        public string ArticleUrl { get; set; }
    }
}
