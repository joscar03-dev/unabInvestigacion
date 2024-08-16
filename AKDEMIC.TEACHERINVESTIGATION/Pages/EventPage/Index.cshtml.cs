using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages.EventPage
{
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;

        public IndexModel(
            AkdemicContext context
        )
        {
            _context = context;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetEventsAsync()
        {

            var result = await _context.Events
                .Select(x => new EventViewModel
                {
                    Title = x.Title,
                    PicturePath = x.PicturePath,
                    Id = x.Id,
                    EventDate = x.EventDate.ToLocalDateTimeFormat(),
                    Organizer = x.Organizer,
                    Cost = x.Cost == 0 ? "Gratuito" : $"Costo : {x.Cost}"
                })
                .ToListAsync();

            return Partial("EventPage/Partials/_EventPartial", result);
        }
    }
}
