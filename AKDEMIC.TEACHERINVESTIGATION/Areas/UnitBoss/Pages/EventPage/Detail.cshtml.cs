using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.ViewModels.EventViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.Pages.EventPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT + "," +
GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT + "," +
GeneralConstants.ROLES.PUBLICATION_UNIT + "," +
GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;

        public DetailModel(
    AkdemicContext context,
                IDataTablesService dataTablesService,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IHttpClientFactory clientFactory
)
        {
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
        }

        public EventDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid eventId)
        {
            var currentEvent = await _context.Events
                .Where(x => x.Id == eventId)
                .Select(x => new EventDetailViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    EventDate = x.EventDate.ToLocalDateTimeFormat(),
                    Description = x.Description,
                    PicturePath = x.PicturePath,
                    Cost = x.Cost == 0 ? "Gratuito" : $"Costo : {x.Cost}",
                    Organizer = x.Organizer,
                    VideoUrl = x.VideoUrl,
                    UserSigned = "",
                    Unit = x.UnitId != null ? x.Unit.Name : "",
                    SignedUp = false
                })
                .FirstOrDefaultAsync();

            if (currentEvent == null)
            {
                return RedirectToPage("/Pages/Index");
            }

            Input = currentEvent;

            return Page();
        }

        public async Task<IActionResult> OnGetEventParticipantDatatableAsync(Guid eventId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<EventParticipant, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "1":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Email;
                    break;
            }

            var query = _context.EventParticipants
                .Where(x => x.EventId == eventId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Email
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }
    }
}
