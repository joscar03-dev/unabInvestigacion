using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text.Json;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Controllers;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //private readonly RoleManager<ApplicationRole> _roleManager;
        protected readonly AkdemicContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;

        public LoginModel(
            SignInManager<ApplicationUser> signInManager,
            //RoleManager<ApplicationRole> roleManager,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IHttpClientFactory clientFactory)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //_roleManager = roleManager;
            _clientFactory = clientFactory;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public bool AllowRegistration { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated) return RedirectToPage("/Index");

            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;

            var allowRegistrationRequest = await _context.Configurations
                .Where(x => x.Key == ConfigurationConstants.TEACHERINVESTIGATION.ALLOW_REGISTRATION_REQUEST)
                .FirstOrDefaultAsync();

            bool isAllowed = false;

            if (allowRegistrationRequest != null)
            {
                _ = bool.TryParse(allowRegistrationRequest.Value, out isAllowed);
            }

            AllowRegistration = isAllowed;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {

            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                //Request a la otra base de datos, por webservice 
                var client = _clientFactory.CreateClient("akdemic");


                //Obtener el token de la aplicacion
                var tokenContent = new StringContent(JsonSerializer.Serialize(new { clientId = "akdemic", clientSecret = "Educacion2020" }), System.Text.Encoding.UTF8, "application/json");
                var tokenResponse = await client.PostAsync(tokenContent, "api/Auth/request-token");
                if (!tokenResponse.IsSuccessStatusCode)
                    // return new BadRequestObjectResult("Fallo el servicio");

                    Console.WriteLine("bandera");

                using var tokenStream = await tokenResponse.Content.ReadAsStreamAsync();

                if (1 == 1)
                {
                    Console.WriteLine("bandera");

                    /*if (loginResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // return new BadRequestObjectResult("ERROR: Contraseña incorrecta 4");
                    }
                    */

                    var user = await _context.Users.Where(x => x.UserName == Input.UserName).FirstOrDefaultAsync();

                    if (user == null)

                    {

                        return new BadRequestObjectResult("ERROR: Contraseña incorrecta");
                    }

                    var auth = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, false, lockoutOnFailure: false);
                    if (auth.Succeeded)
                    {
                        if (user.FirstLoginDate == null)
                        {
                            user.FirstLoginDate = DateTime.UtcNow;
                            await _context.SaveChangesAsync();
                        }

                        return new OkObjectResult(returnUrl ?? "/");


                    }
                    else
                    {
                        return new BadRequestObjectResult("ERROR: Contraseña incorrecta");
                    }
                }

                var tokenModel = await JsonSerializer.DeserializeAsync
                    <TokenViewModel>(tokenStream);

                //Obtener información del usuario despues del login en el otro sistema
                var loginContent = new StringContent(JsonSerializer.Serialize(new { userName = Input.UserName, password = Input.Password }), System.Text.Encoding.UTF8, "application/json");



                var loginResponse = await client.PostAsync(loginContent, tokenModel.token, "api/Auth/request-token"); //"api/Auth/user-login"


                //Si falla el servicio logeamos por nuestra bd, con usuarios no vinculados

                if (!loginResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("bandera");

                    if (loginResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        // return new BadRequestObjectResult("ERROR: Contraseña incorrecta 4");
                    }


                    var user = await _context.Users.Where(x => x.UserName == Input.UserName).FirstOrDefaultAsync();

                    if (user == null)

                    {

                        return new BadRequestObjectResult("ERROR: Contraseña incorrecta");
                    }

                    var auth = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password, false, lockoutOnFailure: false);
                    if (auth.Succeeded)
                    {
                        if (user.FirstLoginDate == null)
                        {
                            user.FirstLoginDate = DateTime.UtcNow;
                            await _context.SaveChangesAsync();
                        }

                        return new OkObjectResult(returnUrl ?? "/");


                    }
                    else
                    {
                        return new BadRequestObjectResult("ERROR: Contraseña incorrecta");
                    }
                }

                using var loginStream = await loginResponse.Content.ReadAsStreamAsync();

                var authenticationUser = await JsonSerializer.DeserializeAsync
                        <AuthenticationUserViewModel>(loginStream);



                //Buscar por AuthenticationUserId
                var validateUser = await _context.Users.IgnoreQueryFilters().Where(x => x.AuthenticationUserId == authenticationUser.authenticationUserId).FirstOrDefaultAsync();

                //Si existe - solo logeas
                if (validateUser != null)
                {
                    if (validateUser.DeletedBy != null && validateUser.DeletedAt != null)
                    {
                        return new BadRequestObjectResult("Sucedio un Error");
                    }

                    await _signInManager.SignInAsync(validateUser, false);
                    if (validateUser.FirstLoginDate == null)
                    {
                        validateUser.FirstLoginDate = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                    }
                    return new OkObjectResult(returnUrl ?? "/");
                }
                else
                {
                    var usernameExist = await _context.Users.IgnoreQueryFilters().AnyAsync(x => x.UserName == authenticationUser.userName);

                    if (usernameExist)
                    {
                        return new BadRequestObjectResult("Sucedio un Error");
                    }

                    //Le pone los mismos roles que existan en ambas bd
                    var dbRoles = await _context.Roles
                        .Select(x => x.NormalizedName)
                        .ToListAsync();

                    var user = new ApplicationUser
                    {
                        Name = authenticationUser.name,
                        PaternalSurname = authenticationUser.paternalSurname,
                        MaternalSurname = authenticationUser.maternalSurname,
                        FullName = $"{authenticationUser.paternalSurname} {authenticationUser.maternalSurname} {authenticationUser.name}",
                        Dni = authenticationUser.dni,
                        Email = authenticationUser.email,
                        UserName = authenticationUser.userName,
                        Address = authenticationUser.address,
                        AuthenticationUserId = authenticationUser.authenticationUserId
                    };

                    var res = await _userManager.CreateAsync(user, Input.Password);

                    if (!res.Succeeded)
                    {
                        return new BadRequestObjectResult("Sucedio un Error");
                    }

                    var currentDbRolesForUser = new List<string>();
                    //Si el que logea tiene rol de Alumnos o Docentes en AKDEMIC, tendra el rol de Investigador
                    for (int i = 0; i < authenticationUser.roles.Count; i++)
                    {
                        var roleFromAkdemic = authenticationUser.roles[i].ToUpper();

                        if (roleFromAkdemic == GeneralConstants.ROLES.TEACHERS.ToUpper())
                        {
                            currentDbRolesForUser.Add(GeneralConstants.ROLES.RESEARCHERS);
                        }

                        if (roleFromAkdemic == GeneralConstants.ROLES.STUDENTS.ToUpper())
                        {
                            currentDbRolesForUser.Add(GeneralConstants.ROLES.RESEARCHERS);
                        }


                        if (dbRoles.Contains(roleFromAkdemic))
                        {
                            currentDbRolesForUser.Add(authenticationUser.roles[i]);
                        }
                    }

                    var currentDbRolesForUserDistinct = currentDbRolesForUser.Distinct().ToList();

                    if (currentDbRolesForUserDistinct.Count > 0)
                    {
                        var result = await _userManager.AddToRolesAsync(user, currentDbRolesForUserDistinct);
                    }

                    var auth = await _signInManager.PasswordSignInAsync(Input.UserName, Input.Password, false, lockoutOnFailure: false);
                    if (auth.Succeeded)
                    {

                        if (user.FirstLoginDate == null)
                        {
                            user.FirstLoginDate = DateTime.UtcNow;
                            await _context.SaveChangesAsync();
                        }

                        return new OkObjectResult(returnUrl ?? "/");
                    }
                    else
                    {
                        return new BadRequestObjectResult("ERROR: Contraseña incorrecta");
                    }


                }
            }

            // If we got this far, something failed, redisplay form
            return new BadRequestObjectResult("ERROR: Contraseña incorrecta");
        }
    }
}
