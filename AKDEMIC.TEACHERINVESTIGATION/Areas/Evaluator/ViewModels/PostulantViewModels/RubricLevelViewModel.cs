using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels
{
    public class RubricLevelViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public decimal Score { get; set; }
    }
}
