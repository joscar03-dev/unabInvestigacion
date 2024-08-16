using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.General.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.UserViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.UserPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        protected readonly AkdemicContext _context;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly IHttpClientFactory _clientFactory;

        public CreateModel(
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            AkdemicContext context,
            IHttpClientFactory clientFactory
        )
        {
            _userManager = userManager;
            _context = context;
            _clientFactory = clientFactory;
            _storageCredentials = storageCredentials;
        }

        [BindProperty]
        public UserCreateViewModel Input { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            //Validacion en nuestra bd
            var existsUser = await _context.Users.IgnoreQueryFilters().Where(x => x.UserName == Input.UserName).AnyAsync();

            //Validacion en el API
            //Request a la otra base de datos, por webservice 
            var client = _clientFactory.CreateClient("akdemic");

            //Obtener el token de la aplicacion
            var tokenContent = new StringContent(JsonSerializer.Serialize(new { clientId = "akdemic", clientSecret = "Educacion2020" }), System.Text.Encoding.UTF8, "application/json");
            var tokenResponse = await client.PostAsync(tokenContent, "api/Auth/request-token");
            if (!tokenResponse.IsSuccessStatusCode)
                return new BadRequestObjectResult("Fallo el servicio");

            using var tokenStream = await tokenResponse.Content.ReadAsStreamAsync();
            var tokenModel = await JsonSerializer.DeserializeAsync
                <TokenViewModel>(tokenStream);

            //Obtener información del usuario despues del login en el otro sistema            
            var userExistsRequest = new HttpRequestMessage(HttpMethod.Get, $"api/ApplicationUser/exists?userName={Input.UserName}");
            userExistsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenModel.token);

            var userExistsResponse = await client.SendAsync(userExistsRequest);

            if (!userExistsResponse.IsSuccessStatusCode)
            {
                return BadRequest("Fallo el servicio");
            }

            using var responseStream = await userExistsResponse.Content.ReadAsStreamAsync();
            var existsUserIntranet = await JsonSerializer.DeserializeAsync
                    <bool>(responseStream);

            if (existsUser || existsUserIntranet)
                return new BadRequestObjectResult("El nombre del usuario ya esta en uso");

            var storage = new CloudStorageService(_storageCredentials);

            DateTime? birthDate = null;
            string pictureUrl = null;
            if (Input.PictureFile != null)
            {
                pictureUrl = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.USERPICTURE,
                        Path.GetExtension(Input.PictureFile.FileName), FileStorageConstants.SystemFolder.GENERAL);
            }


            if (!string.IsNullOrEmpty(Input.BirthDate))
            {
                //Fecha de nacimiento no va como utc
                birthDate = ConvertHelpers.DatepickerToUtcDateTime(Input.BirthDate);
            }

            var user = new ApplicationUser
            {
                Name = Input.Name,
                PaternalSurname = Input.PaternalSurname,
                MaternalSurname = Input.MaternalSurname,
                FullName = $"{Input.PaternalSurname} {Input.MaternalSurname} {Input.Name}",
                Email = Input.Email,
                Dni = Input.Dni,
                UserName = Input.UserName,
                PhoneNumber = Input.PhoneNumber,
                BirthDate = birthDate,
                Address = Input.Address,
                Picture = pictureUrl,
            };

            var identityResult = await _userManager.CreateAsync(user, user.Dni);


            if (Input.RolesId != null)
            {
                if (!identityResult.Succeeded)
                    return new BadRequestObjectResult("Ha fallado la creación del usuario");

                var normalizeRoles = await _context.Roles.Where(x => Input.RolesId.Contains(x.Id))
                    .Select(x => x.NormalizedName).ToListAsync();

                var roleResult = await _userManager.AddToRolesAsync(user, normalizeRoles);
                if (!roleResult.Succeeded)
                    return new BadRequestObjectResult("Ha fallado la asignación del rol al usuario");

            }



            return new OkResult();
        }
    }
}
