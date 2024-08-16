using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string InscriptionStartDate { get; set; }
        public string InscriptionEndDate { get; set; }
        public decimal MinScore { get; set; }
        public string State { get; set; }
        public bool HasRubricQualifications { get; set; }
    }
}
