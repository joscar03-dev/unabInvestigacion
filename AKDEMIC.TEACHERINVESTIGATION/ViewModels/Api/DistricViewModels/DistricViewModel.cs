using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.DistricViewModels
{
    public class DistricViewModel
    {
        public Guid id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public Guid provinceId { get; set; }
        public string provinceName { get; set; }
    }
}
