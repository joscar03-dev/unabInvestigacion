using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.General
{
    public class ApplicationUser: IdentityUserEntity , IKeyNumber, ISoftDelete, ITimestamp, IAggregateRoot
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string MaternalSurname { get; set; }

        [Required]
        [MaxLength(250)]
        public string PaternalSurname { get; set; }

        [MaxLength(750)]
        public string FullName { get; set; }

        public string Picture { get; set; }
        public int Sex { get; set; }

        public int Type { get; set; }  // CORE.GeneralConstants.USER_TYPES.NOT_ASIGNED

        public DateTime? BirthDate { get; set; }
        public DateTime? FirstLoginDate { get; set; }

        [MaxLength(8)]
        public string Dni { get; set; }

        public string Address { get; set; }

        public string CurriculumVitaeUrl { get; set; }
        //Link de la pagina de concytec
        public string CteVitaeConcytecUrl { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public ICollection<Publication> Publications { get; set; }

        public string AuthenticationUserId { get; set; }

    }
}
