using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationQuestionEditViewModel
    {
        public Guid Id { get; set; }
        public bool IsRequired { get; set; }
        public int QuestionType { get; set; }
        public string Description { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }
        public List<InvestigationAnswerEditViewModel> InvestigationAnswers { get; set; }

    }

    public class InvestigationAnswerEditViewModel
    {
        public string Description { get; set; }
    }
}
