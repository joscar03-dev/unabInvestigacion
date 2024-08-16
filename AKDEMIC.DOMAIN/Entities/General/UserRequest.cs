using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.General
{
    public class UserRequest : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string MaternalSurname { get; set; }

        [Required]
        [MaxLength(250)]
        public string PaternalSurname { get; set; }

        [Required]
        [MaxLength(8)]
        public string Dni { get; set; }

        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int State { get; set; } //AKDEMIC.CORE.Constants.GeneralConstants.USERREQUEST_STATES
        public int Type { get; set; } //AKDEMIC.CORE.Constants.GeneralConstants.USERREQUEST_TYPE
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

    }
}
