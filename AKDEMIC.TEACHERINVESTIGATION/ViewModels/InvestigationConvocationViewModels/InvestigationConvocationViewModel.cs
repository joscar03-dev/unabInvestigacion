using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationConvocationViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string PicturePath { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class InvestigationConvocationDetailViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string InscriptionEndDate { get; set; }
        public string InscriptionStartDate { get; set; }
        public string PicturePath { get; set; }
        public string Description { get; set; }
        public string UserSigned { get; set; }
        public bool SignedUp { get; set; }
    }

    public class InvestigationConvocationPostulantViewModel
    {
        public Guid InvestigationConvocationId { get; set; }
    }
}
