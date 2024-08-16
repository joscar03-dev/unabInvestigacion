using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.General
{
    public class Configuration: IAggregateRoot
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
