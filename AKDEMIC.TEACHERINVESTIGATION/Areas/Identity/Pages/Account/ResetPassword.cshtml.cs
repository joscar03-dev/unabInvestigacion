using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Identity.Pages.Account
{
    public class ResetPasswordModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmPassword { get; set; }

            public string Code { get; set; }
        }

        public IActionResult OnGet(string code = null)
        {
            if (code == null)
            {
                throw new ApplicationException("Se debe proporcionar un codigo para restablecer la contraseña");
            }

            Input = new InputModel { Code = code };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() 
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _context.Users.Where(x => x.UserName == Input.UserName).FirstOrDefaultAsync();
            if (user == null)
            {
                // Don't reveal that the user does not exist
                TempData["message"] = "Revise la informción ingresada";
                return RedirectToPage("/Account/ResetPassword", new { code = Input.Code });
            }
            var result = await _userManager.ResetPasswordAsync(user, Input.Code, Input.Password);
            if (result.Succeeded)
            {
                return RedirectToPage("/Account/ResetPasswordConfirmation");
            }
            else
            {
                TempData["message"] = "Token inválido";
                return RedirectToPage("/Account/ResetPassword", new { code = Input.Code });
            }
        }
    }
}
