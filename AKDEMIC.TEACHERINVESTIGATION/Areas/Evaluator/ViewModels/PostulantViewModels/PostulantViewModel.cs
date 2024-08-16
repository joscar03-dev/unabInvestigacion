using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels
{
    public class PostulantViewModel
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public Guid ConvocationId { get; set; }
        public string ConvocationName { get; set; }
        public string ConvocationCode { get; set; }
        public string CreatedAt { get; set; }
        public int ReviewState { get; set; }
        public string ReviewStateText { get; set; }

        public List<RubricSectionViewModel> Sections { get; set; }
    }
}
