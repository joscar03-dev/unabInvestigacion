using AKDEMIC.DOMAIN.Base.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AKDEMIC.DOMAIN.Entities.General
{
    public class ApplicationUserRole : IdentityUserRole<string>, IAggregateRoot
    {
        public virtual ApplicationUser User { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
