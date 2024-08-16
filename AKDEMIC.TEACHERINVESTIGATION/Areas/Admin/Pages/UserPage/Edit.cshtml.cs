using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.UserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.UserPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class EditModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EditModel(
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials,
            AkdemicContext context
)
        {
            _userManager = userManager;
            _context = context;
            _storageCredentials = storageCredentials;
        }

        [BindProperty]
        public UserEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(string userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId)
            .Select(x => new
            {
                UserId = x.Id,
                x.UserName,
                Name = x.Name,
                PaternalSurname = x.PaternalSurname,
                MaternalSurname = x.MaternalSurname,
                Dni = x.Dni,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                Picture = x.Picture,
                BirthDate = x.BirthDate.ToLocalDateFormat(),
                RolesId = x.UserRoles.Select(y => y.RoleId).ToList()

            }).FirstOrDefaultAsync();


            if (user == null)
                return RedirectToPage("/Index");

            Input = new UserEditViewModel
            {
                UserId= user.UserId,
                UserName = user.UserName,
                Name = user.Name,
                PaternalSurname = user.PaternalSurname,
                MaternalSurname = user.MaternalSurname,
                Dni = user.Dni,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                BirthDate=user.BirthDate,
                Address = user.Address,
                Picture = user.Picture,
                RolesId = user.RolesId,
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var storage = new CloudStorageService(_storageCredentials);

            DateTime? birthDate = null;
            string pictureUrl = null;

            var user = await _context.Users.Where(x => x.Id == Input.UserId).FirstOrDefaultAsync();

            if (user == null)
                return new BadRequestObjectResult("Sucedio un error");            

            if (Input.PictureFile != null)
            {
                pictureUrl = await storage.UploadFile(Input.PictureFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.USERPICTURE,
                        Path.GetExtension(Input.PictureFile.FileName), FileStorageConstants.SystemFolder.GENERAL);
                user.Picture = pictureUrl;
            }

            if (string.IsNullOrEmpty(Input.Email))
            {
                return new BadRequestObjectResult("Debe ingresar un correo");
            }

            if (!string.IsNullOrEmpty(Input.BirthDate))
            {
                //Fecha de nacimiento no va como utc
                birthDate = ConvertHelpers.DatepickerToUtcDateTime(Input.BirthDate);
            }

            user.Name = Input.Name;
            user.PaternalSurname = Input.PaternalSurname;
            user.MaternalSurname = Input.MaternalSurname;
            user.FullName = $"{Input.PaternalSurname} {Input.MaternalSurname} {Input.Name}";
            user.Email = Input.Email;
            user.NormalizedEmail = Input.Email.ToUpper();
            user.PhoneNumber = Input.PhoneNumber;
            user.BirthDate = birthDate;
            user.Address = Input.Address;
            user.Dni = Input.Dni;

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles != null && userRoles.Count > 0)
            {
                var deleterole = await _userManager.RemoveFromRolesAsync(user, userRoles);

                if (!deleterole.Succeeded)
                    return new BadRequestObjectResult("Sucedio un error");
            }



            if (Input.RolesId != null && Input.RolesId.Count > 0)
            {
                var roles = await _context.Roles.Where(x => Input.RolesId.Contains(x.Id))
                    .Select(x => x.NormalizedName).ToListAsync();

                if (roles.Count > 0 && roles != null)
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, roles);
                    if (!roleResult.Succeeded)
                        return new BadRequestObjectResult("Ha fallado la asignación del rol al usuario");
                }

            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
