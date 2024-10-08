﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationTypeViewModels
{
    public class InvestigationTypeEditViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
