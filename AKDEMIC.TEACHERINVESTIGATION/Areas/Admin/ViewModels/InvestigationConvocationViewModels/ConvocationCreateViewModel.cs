using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class ConvocationCreateViewModel
    {
      
        [StringLength(900)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(3000)]
        public string Description { get; set; }

        public IFormFile PictureFile { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string InscriptionStartDate { get; set; }

        public string InscriptionEndDate { get; set; }

        public decimal MinScore { get; set; }

        public byte State { get; set; }

        public bool AllowInquiries { get; set; }

        public string InquiryStartDate { get; set; }

        public string InquiryEndDate { get; set; }

    }

   
}
