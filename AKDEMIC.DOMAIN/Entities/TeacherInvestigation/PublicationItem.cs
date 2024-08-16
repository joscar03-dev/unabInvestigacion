using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PublicationItem : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Required]
        public Guid IdItem { get; set; }
        [Required]
        public Guid? PublicationId { get; set; }    
        public string Valor { get; set; } 

    }
}
