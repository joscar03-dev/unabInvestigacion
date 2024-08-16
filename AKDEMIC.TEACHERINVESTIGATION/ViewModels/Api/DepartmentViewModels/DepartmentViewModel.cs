using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.DepartmentViewModels
{
    public class DepartmentViewModel
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public Guid? countryId { get; set; }
        public string countryName { get; set; }
    }
}
