using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.IncubatorPostulationViewModels
{
    public class IncubatorRubricCriterionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MaxScore { get; set; }
        public decimal? Qualification { get; set; }

        public List<IncubatorRubricLevelViewModel> Levels { get; set; }
    }
}
