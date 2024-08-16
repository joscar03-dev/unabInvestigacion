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
    public class EditModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ICloudStorageService _cloudStorageService;
        private readonly AkdemicContext _context;

        public EditModel(
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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var user = await _context.Users.Where(x => x.Id == id).FirstOrDefaultAsync();
            
            ExternalUser = new ExternalUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                PaternalSurname =user.PaternalSurname,
                MaternalSurname = user.MaternalSurname,
                Dni = user.Dni,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName,
                CurriculumVitaeUrl = user.CurriculumVitaeUrl
            };

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync()
        {
            var user = await _context.Users.Where(x => x.Id == ExternalUser.Id).FirstOrDefaultAsync();

            user.Name = ExternalUser.Name;
            user.PaternalSurname = ExternalUser.PaternalSurname;
            user.MaternalSurname = ExternalUser.MaternalSurname ?? "";
            user.Dni = ExternalUser.Dni;
            user.PhoneNumber = ExternalUser.PhoneNumber;
            user.Email = ExternalUser.Email;
            user.FullName = $"{ExternalUser.Name} {ExternalUser.PaternalSurname} {ExternalUser.MaternalSurname}".Trim();

            if(ExternalUser.CurriculumVitaeFile != null)
            {
                var cv_url = await _cloudStorageService.UploadFile(
                    ExternalUser.CurriculumVitaeFile.OpenReadStream(),
                    FileStorageConstants.CONTAINER_NAMES.USER_CURRICULUM_VITAE,
                    Path.GetExtension(ExternalUser.CurriculumVitaeFile.FileName),
                    FileStorageConstants.SystemFolder.GENERAL
                    );

                user.CurriculumVitaeUrl = cv_url;
            }

            await _userManager.UpdateAsync(user);
            return new OkResult();
        }
    }
}
