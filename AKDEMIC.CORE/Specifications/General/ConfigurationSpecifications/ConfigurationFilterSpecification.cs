using AKDEMIC.DOMAIN.Entities.General;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.General.ConfigurationSpecifications
{
    public sealed class ConfigurationFilterSpecification : Specification<Configuration>
    {
        public ConfigurationFilterSpecification(string key)
        {
            Query.Where(x => x.Key == key);
        }
    }
}
