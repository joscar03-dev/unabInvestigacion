using System;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationConvocationObservationCreateModel
    {
        public int State { get; set; }
        public string Description { get; set; }
        public Guid investigationconvocationpostulantId { get; set; }
       
    }
}
