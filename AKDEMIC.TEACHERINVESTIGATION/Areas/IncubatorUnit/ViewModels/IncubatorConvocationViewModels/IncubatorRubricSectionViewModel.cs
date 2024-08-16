using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{

        public class IncubatorRubricSectionViewModel
        {
            public Guid? Id { get; set; }
            public string Title { get; set; }
            public decimal MaxSectionScore { get; set; }
            public Guid ConvocationId { get; set; }

            public List<IncubatorRubricCriterionViewModel> RubricCriterions { get; set; }
        }

        public class IncubatorRubricViewModel
        {
            public bool HasQualifications { get; set; }
            public List<IncubatorRubricSectionViewModel> RubricSections { get; set; }
        }

}
