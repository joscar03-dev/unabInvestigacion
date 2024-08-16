using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.FacultyViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorConvocationEditViewModel
    {
        public Guid IncubatorConvocationId { get; set; }

        [StringLength(50)]
        public string Code { get; set; }

        [StringLength(900)]
        public string Name { get; set; }

        public IFormFile PictureFile { get; set; }

        public string PicturePath { get; set; }
        public IFormFile DocumentFile { get; set; }

        public string DocumentPath { get; set; }

        public string StartDate { get; set; }

        public string EndDate { get; set; }

        public string InscriptionStartDate { get; set; }

        public string InscriptionEndDate { get; set; }

        public string AddresedTo { get; set; }

        public string Requirements { get; set; }

        public List<FacultyViewModel> Faculties { get; set; }

        public int TotalWinners { get; set; }
    }
}
