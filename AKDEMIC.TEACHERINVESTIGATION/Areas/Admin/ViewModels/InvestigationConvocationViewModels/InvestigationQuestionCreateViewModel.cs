using System;
using System.Collections.Generic;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationQuestionCreateViewModel 
    {
        public bool IsRequired { get; set; }
        public int QuestionType { get; set; }
        public string Description { get; set; }
        public Guid InvestigationConvocationRequirementId { get; set; }
        public List<InvestigationAnswerCreateViewModel> Answers { get; set; }
        
    }
    public class InvestigationAnswerCreateViewModel 
    {
        public string Description { get; set; }       
    }
}
