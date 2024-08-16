using AKDEMIC.DOMAIN.Base.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.General
{
    public class ApplicationRole :IdentityRole, IAggregateRoot
    {
        public int Priority { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
