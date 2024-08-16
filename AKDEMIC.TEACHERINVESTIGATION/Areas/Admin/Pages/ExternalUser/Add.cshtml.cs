using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ExternalUserViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.ExternalUser
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN)]
    public class AddModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly AkdemicContext _context;

        public AddModel(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ICloudStorageService cloudStorageService,
            AkdemicContext context
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _cloudStorageService = cloudStorageService;
            _context = context;
        }

        [BindProperty]
        public ExternalUserViewModel ExternalUser { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!await _roleManager.RoleExistsAsync(GeneralConstants.ROLES.EXTERNAL_EVALUATOR))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Name = GeneralConstants.ROLES.EXTERNAL_EVALUATOR
                });
            }

            if (await _context.Users.AnyAsync(y => y.UserName.ToLower().Trim() == ExternalUser.UserName))
                return new BadRequestObjectResult($"Ya existe un usuario con el código {ExternalUser.UserName}.");

            if (string.IsNullOrEmpty(ExternalUser.Dni))
                return new BadRequestObjectResult("El campo DNI es obligatorio.");

            var cv_url = await _cloudStorageService.UploadFile(
                ExternalUser.CurriculumVitaeFile.OpenReadStream(),
                FileStorageConstants.CONTAINER_NAMES.USER_CURRICULUM_VITAE,
                Path.GetExtension(ExternalUser.CurriculumVitaeFile.FileName),
                FileStorageConstants.SystemFolder.GENERAL
                );

            var entity = new ApplicationUser
            {
                Name = ExternalUser.Name,
                PaternalSurname = ExternalUser.PaternalSurname,
                MaternalSurname = ExternalUser.MaternalSurname ?? "",
                Dni = ExternalUser.Dni,
                PhoneNumber = ExternalUser.PhoneNumber,
                Email = ExternalUser.Email,
                UserName = ExternalUser.UserName,
                CurriculumVitaeUrl = cv_url,
                FullName = $"{ExternalUser.Name} {ExternalUser.PaternalSurname} {ExternalUser.MaternalSurname}".Trim()
            };

            var result = await _userManager.CreateAsync(entity, ExternalUser.Dni);

            if (!result.Succeeded)
                return new BadRequestObjectResult(string.Join(" ,", result.Errors.Select(x=>x.Description).ToList()));

            await _userManager.AddToRoleAsync(entity, GeneralConstants.ROLES.EXTERNAL_EVALUATOR);

            return new OkResult();
        }
    }
}
