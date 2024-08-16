using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels
{
    public class RubricCriterionViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MaxScore { get; set; }
        public decimal? Qualification { get; set; }

        public List<RubricLevelViewModel> Levels { get; set; }
    }
}
