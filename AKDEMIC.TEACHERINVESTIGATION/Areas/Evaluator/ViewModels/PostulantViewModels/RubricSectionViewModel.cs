using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels
{
    public class RubricSectionViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public decimal MaxSectionScore { get; set; }
        public Guid ConvocationId { get; set; }

        public List<RubricCriterionViewModel> RubricCriterions { get; set; }
    }
}
