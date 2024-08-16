using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationQuestionViewModel
    {
        public Guid Id { get; set; }
        public bool IsRequired { get; set; }
        public int QuestionType { get; set; }
        public string Description { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }
        public List<InvestigationAnswerViewModel> InvestigationAnswers { get; set; }
    }

    public class InvestigationAnswerViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
    }
}
