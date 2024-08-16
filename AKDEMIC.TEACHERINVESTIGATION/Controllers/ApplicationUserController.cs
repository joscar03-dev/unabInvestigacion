using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.General.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        protected readonly AkdemicContext _context;
        private readonly ISelect2Service _select2Service;
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationUserController(
            AkdemicContext context,
            ISelect2Service select2Service,
            IApplicationUserService applicationUserService,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _select2Service = select2Service;
            _applicationUserService = applicationUserService;
            _userManager = userManager;
        }

        [HttpGet("select-search")]
        public async Task<IActionResult> SearchUser(string term = null,int userType = -1)
        {
            var query = _context.Users.AsNoTracking();
            //Si esta logeado, no mostrar el usuario logeado
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser != null)
            {
                query = query.Where(x => x.Id != currentUser.Id);
            }

            //Distinto de todos
            if (userType != -1)
            {
                query = query.Where(x => x.Type == userType);
            }

            if (!string.IsNullOrEmpty(term))
            {
                string serachValue = term.Trim();

                query = query.Where(x => x.FullName.ToUpper().Contains(serachValue.ToUpper()) ||
                    x.UserName.ToUpper().Contains(serachValue.ToUpper()) ||
                    x.PaternalSurname.ToUpper().Contains(serachValue.ToUpper()) ||
                    x.MaternalSurname.ToUpper().Contains(serachValue.ToUpper()) ||
                    x.Name.ToUpper().Contains(serachValue.ToUpper()));
            }
           
            var result = await query
                .OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    text = $"{x.UserName} - {x.FullName}"
                })
                .Take(5)
                .ToListAsync();

            return Ok(result);
        }

        [HttpGet("select/evaluadores-externos")]
        public async Task<IActionResult> GetExternalEvaluators()
        {
            var parameters = _select2Service.GetRequestParameters();
            var selectedRoles = new List<string>
            {
                GeneralConstants.ROLES.EXTERNAL_EVALUATOR
            };

            var result = await _applicationUserService.GetSelect2(parameters, null, selectedRoles);
            return Ok(result);
        }
    }
}
