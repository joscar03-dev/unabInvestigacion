using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectViewModel
    {
        public Guid InvestigationProjectId { get; set; }
        public Guid InvestigationConvocationPostulantId { get; set; }
        public string InvestigationProjectType { get; set; }
        public string GanttDiagramUrl { get; set; }
        public string ExecutionAddress { get; set; }
        public string ProjectTitle { get; set; }
        public string GeneralGoal { get; set; }
        public string SpecificGoal { get; set; }
        public string FullName { get; set; }
        public string FinancingInvestigation { get; set; }
    }
}
