using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages.InvestigationConvocationPage
{
    public class IndexModel : PageModel
    {
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;

        public IndexModel(
            IAsyncRepository<InvestigationConvocation> investigationConvocationRepository
        )
        {
            _investigationConvocationRepository = investigationConvocationRepository;
        }
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnGetInvestigationConvocationsAsync()
        {
            var investigationConvocations = await _investigationConvocationRepository.ListAllAsync();

            var result = investigationConvocations
                .Select(x => new InvestigationConvocationViewModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    PicturePath = x.PicturePath,
                    Id = x.Id,
                    Description = x.Description,
                    StartDate = x.StartDate.ToDateFormat(),
                    EndDate = x.EndDate.ToDateFormat(),
                })
                .ToList();

            return Partial("InvestigationConvocationPage/Partials/_ConvocationPartial", result);
        }
    }
}
