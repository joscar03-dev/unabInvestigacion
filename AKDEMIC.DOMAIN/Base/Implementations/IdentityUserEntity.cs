﻿using AKDEMIC.DOMAIN.Base.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AKDEMIC.DOMAIN.Base.Implementations
{
    public class IdentityUserEntity : IdentityUser, IBaseEntity
    {
        [NotMapped]
        public int GeneratedId { get; set; }

        [NotMapped]
        public string RelationId { get; set; }

        [NotMapped]
        public DateTime? DeletedAt { get; set; }

        [NotMapped]
        public string DeletedBy { get; set; }

        [NotMapped]
        public DateTime? CreatedAt { get; set; }

        [NotMapped]
        public string CreatedBy { get; set; }

        [NotMapped]
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public string UpdatedBy { get; set; }

        [NotMapped]
        public string SearchId { get; set; }
    }
}
