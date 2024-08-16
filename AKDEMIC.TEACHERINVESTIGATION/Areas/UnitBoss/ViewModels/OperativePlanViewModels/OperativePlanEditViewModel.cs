using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.ViewModels.OperativePlanViewModels
{
    public class OperativePlanEditViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public IFormFile File { get; set; }
    }
}
