using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.ProvinceViewModels
{
    public class ProvinceViewModel
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public Guid departmentId { get; set; }
        public string departmentName { get; set; }
    }
}
