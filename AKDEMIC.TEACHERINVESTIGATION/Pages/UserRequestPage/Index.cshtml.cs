using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Helpers;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.ProfileViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.UserRequestViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages.UserRequestPage
{
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;

        public IndexModel(
            AkdemicContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var allowRegistrationRequest = await _context.Configurations
                .Where(x => x.Key == ConfigurationConstants.TEACHERINVESTIGATION.ALLOW_REGISTRATION_REQUEST)
                .FirstOrDefaultAsync();

            if (allowRegistrationRequest == null || !(GeneralConstants.Institution.VALUE == InstitutionConstants.UNAB))
                return RedirectToPage("/Index");

            bool isAllowed = false;
            var result = bool.TryParse(allowRegistrationRequest.Value, out isAllowed);

            if (!result)
                return RedirectToPage("/Index");

            if (!isAllowed)
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(UserRequestViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verifique los campos del formulario");

            if (string.IsNullOrEmpty(viewModel.Dni))
                return new BadRequestObjectResult("Especifique el DNI");

            if (viewModel.Type == -1)
                return new BadRequestObjectResult("Seleccione un Tipo");

            var userRequestExist = await _context.UserRequests
                .AnyAsync(x => x.Dni == viewModel.Dni && x.Type == viewModel.Type);

            if (userRequestExist)
                return new BadRequestObjectResult($"Ya existe una solicitud con el DNI {viewModel.Dni}");

            if (!GeneralConstants.USERREQUEST_TYPE.VALUES.ContainsKey(viewModel.Type))
                return new BadRequestObjectResult("Debe seleccionar un Tipo valido");

            var userRequest = new UserRequest
            {
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Name = viewModel.Name,
                Dni = viewModel.Dni,
                PhoneNumber = viewModel.PhoneNumber,
                State = GeneralConstants.USERREQUEST_STATES.PENDING,
                Type = viewModel.Type,
                Email = viewModel.Email
            };

            await _context.UserRequests.AddAsync(userRequest);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
