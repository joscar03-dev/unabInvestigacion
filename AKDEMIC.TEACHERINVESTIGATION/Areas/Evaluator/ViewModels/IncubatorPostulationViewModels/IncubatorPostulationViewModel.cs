using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.IncubatorPostulationViewModels
{
    public class IncubatorPostulationViewModel
    {
        public Guid Id { get; set; }
        public string ConvocationCode { get; set; }

        public string ConvocationName { get; set; }
        public string PostulantFullName { get; set; }
        public string PostulantCode { get; set; }

        public string Title { get; set; }

        public string GeneralGoals { get; set; }

        public int MonthDuration { get; set; }

        public decimal Budget { get; set; }

        public string DepartmentText { get; set; }

        public string ProvinceText { get; set; }

        public string DistrictText { get; set; }
    }

    public class IncubatorPostulationDetailViewModel : IncubatorPostulationViewModel
    {
        public string Requirements { get; set; }
        public string DocumentPath { get; set; }

        public string InscriptionStartDate { get; set; }
        public string InscriptionEndDate { get; set; }
        //public List<FacultyViewModel> Faculties { get; set; }
    }
}
