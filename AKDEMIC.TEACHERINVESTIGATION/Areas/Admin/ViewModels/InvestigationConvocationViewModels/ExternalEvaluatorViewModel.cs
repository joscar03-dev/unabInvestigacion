using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ExternalEvaluatorViewModel
    {
        public Guid InvestigationConvocationId { get; set; }
        public string UserId { get; set; }
    }
}
