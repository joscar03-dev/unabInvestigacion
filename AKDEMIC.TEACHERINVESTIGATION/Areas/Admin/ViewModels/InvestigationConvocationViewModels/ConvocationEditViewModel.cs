using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationEditViewModel 
    {
        public Guid InvestigationConvocationId { get; set; }

        public string InvestigationConvocationName { get; set; }

        public string InvestigationConvocationCode { get; set; }

        [Display(Name = "Imagen", Prompt = "Imagen")]
        public IFormFile InvestigationConvocationPictureFile { get; set; }

        public string InvestigationConvocationPicturePath { get; set; }

        public string InvestigationConvocationDescription { get; set; }

        public string InvestigationConvocationStartDate { get; set; }

        public string InvestigationConvocationEndDate { get; set; }
        public string InvestigationConvocationInscriptionStartDate { get; set; }

        public string InvestigationConvocationInscriptionEndDate { get; set; }


        public decimal InvestigationConvocationMinScore { get; set; }

        public string InvestigationConvocationState { get; set; }

        public bool InvestigationConvocationAllowInquiries { get; set; }

        public string InvestigationConvocationInquiryStartDate { get; set; }

        public string InvestigationConvocationInquiryEndDate { get; set; }
    }
}
