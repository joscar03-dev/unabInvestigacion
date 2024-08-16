using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Committee.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationConvocationObservationCreateModel
    {
        public int State { get; set; }
        public string Description { get; set; }
        public Guid investigationconvocationpostulantId { get; set; }
    }
}
