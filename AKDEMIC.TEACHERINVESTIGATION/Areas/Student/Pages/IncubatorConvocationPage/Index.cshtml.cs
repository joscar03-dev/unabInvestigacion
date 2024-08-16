using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Student.ViewModels.IncubatorConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Student.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.STUDENTS)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public IndexModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetIncubatorConvocationsAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var today = DateTime.UtcNow;
            var result = await _context.IncubatorConvocations
                .Where(x => x.StartDate <= today &&  x.EndDate >= today)
                .Select(x => new IncubatorConvocationViewModel
                {
                    Code = x.Code,
                    Name = x.Name,
                    PicturePath = x.PicturePath,
                    Id = x.Id,
                    AddressedTo = x.AddressedTo,
                    StartDate = x.StartDate.ToDateFormat(),
                    EndDate = x.EndDate.ToDateFormat(),
                    IsPostulant = x.IncubatorPostulations.Any(y => y.UserId == user.Id)
                })
                .ToListAsync();

            return Partial("IncubatorConvocationPage/Partials/_IncubatorConvocationPartial", result);
        }
    }
}
