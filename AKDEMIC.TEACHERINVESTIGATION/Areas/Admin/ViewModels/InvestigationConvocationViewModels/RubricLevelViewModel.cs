using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class RubricLevelViewModel
    {
        public Guid? Id { get; set; }
        public decimal Score { get; set; }
        public string Description { get; set; }

        public Guid RubricCriterionId { get; set; }
    }
}
