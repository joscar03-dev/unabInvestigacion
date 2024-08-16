using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ExternalUserViewModels
{
    public class ExternalUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Dni { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CurriculumVitaeUrl { get; set; }
        public IFormFile CurriculumVitaeFile { get; set; }
    }
}
