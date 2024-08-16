using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class EvaluatorCommitteeViewModel
    {
        [Required]
        public string UserId { get; set; }
        public Guid InvestigationConvocationId { get; set; }
    }
}
