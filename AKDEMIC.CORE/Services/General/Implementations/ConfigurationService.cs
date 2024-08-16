using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.General.Interfaces;
using AKDEMIC.CORE.Specifications.General.ApplicationUserSpecifications;
using AKDEMIC.CORE.Specifications.General.ConfigurationSpecifications;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.General.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IAsyncRepository<Configuration> _configurationRepository;

        public ConfigurationService(
            IAsyncRepository<Configuration> configurationRepository
            )
        {
            _configurationRepository = configurationRepository;
        }

        public async Task<string> GetValueByKey(string key)
        {
            var userFilterSpecification = new ConfigurationFilterSpecification(key);

            var configuration = await _configurationRepository.FirstOrDefaultAsync(userFilterSpecification);

            string value = null;

            if (configuration != null)
                value = configuration.Value;

            return value;
        }
    }
}
