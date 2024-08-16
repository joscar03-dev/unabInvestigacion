using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorRubricCriterionViewModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid RubricSectionId { get; set; }

        public List<IncubatorRubricLevelViewModel> Levels { get; set; }
    }
}
