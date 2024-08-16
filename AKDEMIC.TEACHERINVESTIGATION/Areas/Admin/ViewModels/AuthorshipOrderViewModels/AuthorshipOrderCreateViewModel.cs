using System.ComponentModel.DataAnnotations;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.AuthorshipOrderViewModels
{
    public class AuthorshipOrderCreateViewModel
    {
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
