using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.IncubatorPostulationViewModels
{
    public class IncubatorPostulantViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public Guid ConvocationId { get; set; }
        public string ConvocationName { get; set; }
        public string ConvocationCode { get; set; }
        public string CreatedAt { get; set; }
        public int ReviewState { get; set; }
        public string ReviewStateText { get; set; }

        public List<IncubatorRubricSectionViewModel> Sections { get; set; }
    }
}
