using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.CareersViewModels
{
    public class CareersViewModel
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public Guid facultyId { get; set; }
        public string facultyName { get; set; }
    }
}
