﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.FinancingInvestigationViewModels
{
    public class FinancingInvestigationEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
