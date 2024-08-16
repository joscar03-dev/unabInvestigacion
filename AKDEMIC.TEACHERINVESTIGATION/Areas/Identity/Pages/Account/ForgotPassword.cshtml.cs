using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSenderService _emailSenderService;

        public ForgotPasswordModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IEmailSenderService emailSenderService
        )
        {
            _context = context;
            _userManager = userManager;
            _emailSenderService = emailSenderService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }

            public string UserName { get; set; }
        }


        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var users = await _context.Users.Where(x => x.Email.ToUpper() == Input.Email.ToUpper()).ToListAsync();

                if (users.Count > 1)
                {
                    if (string.IsNullOrEmpty(Input.UserName))
                    {
                        ViewData["NeedUserName"] = true;
                        ModelState.AddModelError(string.Empty, "Debe ingresar un usuario");
                        //Agregar al state, que debe ingresar un usuario, mostrar el usuario input
                        // If we got this far, something failed, redisplay form
                        return Page();
                    }

                    //Traemos el usuario por username
                    var user = users.Where(x => x.UserName == Input.UserName).FirstOrDefault();

                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        //return RedirectToAction(nameof(ForgotPasswordConfirmation));
                    }

                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(pageName: "/Account/ResetPassword", pageHandler: null, values: new { userid = user.Id, code }, protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Reiniciar contraseña",
                    //     $"Por favor reinicie su contraseña en el siguiente link: <a href='{callbackUrl}'>link</a>");

                    await _emailSenderService.SendEmailPasswordRecoveryAsync(Helpers.ConstantHelpers.PROJECT.NAME, Input.Email, GeneralConstants.GetApplicationRoute(GeneralConstants.Solution.TeacherInvestigation), callbackUrl);
                }
                else if (users.Count == 1)
                {
                    var user = users.FirstOrDefault();

                    if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                    {
                        // Don't reveal that the user does not exist or is not confirmed
                        return RedirectToPage("/Account/ForgotPasswordConfirmation");
                    }

                    // For more information on how to enable account confirmation and password reset please
                    // visit https://go.microsoft.com/fwlink/?LinkID=532713
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Page(pageName: "/Account/ResetPassword", pageHandler: null, values: new { userid = user.Id, code }, protocol: Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Reiniciar contraseña",
                    //     $"Por favor reinicie su contraseña en el siguiente link: <a href='{callbackUrl}'>link</a>");

                    await _emailSenderService.SendEmailPasswordRecoveryAsync(Helpers.ConstantHelpers.PROJECT.NAME, Input.Email, GeneralConstants.GetApplicationRoute(GeneralConstants.Solution.TeacherInvestigation), callbackUrl);
                }
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("/Account/ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
