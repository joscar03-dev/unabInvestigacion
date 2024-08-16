using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorRubricLevelViewModel
    {
        public Guid? Id { get; set; }
        public decimal Score { get; set; }
        public string Description { get; set; }

        public Guid RubricCriterionId { get; set; }
    }
}
