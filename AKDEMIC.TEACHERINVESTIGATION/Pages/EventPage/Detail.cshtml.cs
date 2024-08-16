using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.EventViewModels;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.TEACHERINVESTIGATION.Helpers;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages.EventPage
{
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IQRService _qRService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;

        public DetailModel(
            AkdemicContext context,
            IQRService qRService,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpClientFactory clientFactory
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
            _qRService = qRService;
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

            bool userSignedUp = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userSignedUp = await _context.EventParticipants
                        .AnyAsync(x => x.EventId == currentEvent.Id && x.UserId == user.Id);
                    currentEvent.UserSigned = $"{user.UserName}-{user.FullName}";
                }
            }

            currentEvent.SignedUp = userSignedUp;

            var url = Url.PageLink("Detail");

            Input = currentEvent;
            if (!string.IsNullOrEmpty(url))
            {
                var qrCode = _qRService.GenerateQR(url);
                Input.ImageQR = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(qrCode));
            }

            return Page();
        }

        public async Task<IActionResult> OnPostRegisterToEventAsGuestAsync(EventGuestParticipantViewModel viewModel)
        {
            var today = DateTime.UtcNow;

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var currentEvent = await _context.Events
                .Where(x => x.Id == viewModel.EventId)
                .Select(x => new 
                {
                    x.Id,
                    x.EventDate
                })
                .FirstOrDefaultAsync();

            if (currentEvent == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (currentEvent.EventDate < today)
                return new BadRequestObjectResult("El evento ya no se encuentra disponible");

            if (string.IsNullOrEmpty(viewModel.BirthDate))
                return new BadRequestObjectResult("Debe especificar su fecha de nacimiento");

            var isParticipant = await _context.EventParticipants
                .AnyAsync(x => x.EventId == currentEvent.Id && x.Dni == viewModel.Dni);

            if (isParticipant)
                return new BadRequestObjectResult($"Ya existe un participante registrado con el Dni  {viewModel.Dni}, para este evento");

            var participant = new EventParticipant
            {
                EventId = currentEvent.Id,
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Name = viewModel.Name,
                Dni = viewModel.Dni,
                Email = viewModel.Email,
                University = viewModel.University,
                PhoneNumber = viewModel.PhoneNumber,
                BirthDate = ConvertHelpers.DatepickerToUtcDateTime(viewModel.BirthDate)
            };
            await _context.EventParticipants.AddAsync(participant);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostRegisterToEventAsUserAsync(EventUserParticipantViewModel viewModel)
        {
            var today = DateTime.UtcNow;

            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var currentEvent = await _context.Events
                .Where(x => x.Id == viewModel.EventId)
                .Select(x => new
                {
                    x.Id,
                    x.EventDate
                })
                .FirstOrDefaultAsync();

            if (currentEvent == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (currentEvent.EventDate < today)
                return new BadRequestObjectResult("El evento ya no se encuentra disponible");

            var user = await _userManager.GetUserAsync(User);

            var isParticipant = await _context.EventParticipants
                .AnyAsync(x => x.EventId == currentEvent.Id && x.UserId == user.Id);

            if (isParticipant)
                return new BadRequestObjectResult($"Ya existe un participante registrado con el usuario  {user.UserName}, para este evento");

            var participant = new EventParticipant
            {
                EventId = currentEvent.Id,
                UserId = user.Id
            };
            await _context.EventParticipants.AddAsync(participant);

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
