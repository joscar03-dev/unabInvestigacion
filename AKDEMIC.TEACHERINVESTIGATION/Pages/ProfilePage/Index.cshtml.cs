using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.ProfileViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages.ProfilePage
{
    public class IndexModel : PageModel
    {

        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public UserViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return RedirectToPage("Index");

            Input = new UserViewModel
            {
                Dni = user.Dni,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Picture = user.Picture,
                CTEVitaeUrl = user.CteVitaeConcytecUrl
            };


            return Page();
        }

        public async Task<IActionResult> OnPostUpdateUserInformationAsync(UserInformationViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            if (string.IsNullOrEmpty(viewModel.Email))
            {
                return new BadRequestObjectResult("El correo no puede estar vacio");
            }

            user.PhoneNumber = viewModel.PhoneNumber;
            user.Email = viewModel.Email;
            user.NormalizedEmail = viewModel.Email.ToUpper();
            user.CteVitaeConcytecUrl = viewModel.CTEVitaeUrl;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostChangeUserPasswordAsync(UserPasswordViewModel viewModel)
        {
            if (viewModel.NewPassword != viewModel.RepeatPassword)
            {
                return new BadRequestObjectResult("Las contraseñas no coinciden");
            }
            var user = await _userManager.GetUserAsync(User);

            var passwordValidator = new PasswordValidator<ApplicationUser>();

            var currentPasswordIsCorrect = await _userManager.CheckPasswordAsync(user, viewModel.CurrentPassword);

            var newPasswordIsValid = passwordValidator.ValidateAsync(_userManager, user, viewModel.NewPassword).Result.Succeeded;

            if (!currentPasswordIsCorrect || !newPasswordIsValid)
            {
                return new BadRequestResult();
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, viewModel.NewPassword);

            await _context.SaveChangesAsync();
            return new OkResult();

        }
    }
}
