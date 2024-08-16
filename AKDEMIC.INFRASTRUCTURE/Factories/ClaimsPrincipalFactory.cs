using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.INFRASTRUCTURE.Factories
{
    public class ClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
    {
        private readonly AkdemicContext _context;

        public ClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            AkdemicContext context
        )
        : base(userManager, roleManager, optionsAccessor)
        {
            _context = context;
        }

        public override async Task<ClaimsPrincipal> CreateAsync(ApplicationUser user)
        {
            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
            var roles = await UserManager.GetRolesAsync(user);

            //Putting our Property to Claims

            identity.AddClaims(new[] {
                new Claim(ClaimTypes.UserData, $"{user.FullName}"),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim("PictureUrl", user.Picture ?? ""),
            });

            if (roles.Count > 0)
            {
                var rolePriotiry = await _context.UserRoles
                    .Where(x => x.UserId == user.Id)
                    .OrderByDescending(x => x.Role.Priority)
                    .Select(x => x.Role.Name).FirstOrDefaultAsync();

                identity.AddClaim(new Claim("RolePriorityName", rolePriotiry));
                identity.AddClaim(new Claim("AkdemicPermissions", string.Join(",", roles)));
            }
            else
            {
                identity.AddClaim(new Claim("RolePriorityName", ""));
            }

            //identity.AddClaims(roles.Select(role => new Claim("role", role)));

            return principal;
        }
    }
}
