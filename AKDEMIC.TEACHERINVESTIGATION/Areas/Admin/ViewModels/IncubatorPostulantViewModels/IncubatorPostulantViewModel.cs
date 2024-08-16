using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.IncubatorPostulantViewModels
{
    public class IncubatorPostulantViewModel
    {
        public Guid Id { get; set; }
        public string ConvocationCode { get; set; }
        public string ConvocationName { get; set; }
        public string PostulantFullName { get; set; }
        public string PostulantCode { get; set; }
        public string Title { get; set; }
        public string GeneralGoals { get; set; }
        public decimal Budget { get; set; }
        public int MonthDuration { get; set; }
        public string DepartmentText { get; set; }
        public string ProvinceText { get; set; }
        public string DistrictText { get; set; }
    }

    public class IncubatorPostulantDetailViewModel : IncubatorPostulantViewModel
    {
        public string Requirements { get; set; }
        public string DocumentPath { get; set; }

        public string InscriptionStartDate { get; set; }
        public string InscriptionEndDate { get; set; }
        //public List<FacultyViewModel> Faculties { get; set; }
    }

    public class ScheduleViewModels
    {
        public int MonthDuration { get; set; }
        public List<SpecificGoalViewModel> SpecificGoals { get; set; }
    }

    public class SpecificGoalViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public List<ActivityViewModel> Activities { get; set; }
    }
    public class ActivityViewModel
    {
        public Guid Id { get; set; }
        public Guid IncubatorPostulationSpecificGoalId { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public List<ActivityMonthViewModel> ActivityMonths { get; set; }
    }

    public class ActivityMonthViewModel
    {
        public Guid IncubatorPostulationActivityId { get; set; }
        public int MonthNumber { get; set; }
    }
}
