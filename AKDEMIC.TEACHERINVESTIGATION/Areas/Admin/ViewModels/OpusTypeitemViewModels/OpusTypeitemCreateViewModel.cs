using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.OpusTypeitemViewModels
{
    public class OpusTypeitemCreateViewModel
    {
        public Guid IdType { get; set; }
        public Guid IdItem { get; set; }
        public int Orden { get; set; }
        public string activo { get; set; }
    }
}
