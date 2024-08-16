using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectTaskViewModel
    {
        public Guid InvestigationProjectId { get; set; }
        public string Description { get; set; }
    }

    public class InvestigationProjectTaskEditViewModel
    {
        public Guid Id { get; set; }
        public Guid InvestigationProjectId { get; set; }
        public string Description { get; set; }
    }
}
