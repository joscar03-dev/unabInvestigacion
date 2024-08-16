using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.ViewModels.InvestigationProjectViewModels
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
        public int? Budget { get; set; }

        public string ProjectState { get; set; }
        public string CareerText { get; set; }
    }
}
