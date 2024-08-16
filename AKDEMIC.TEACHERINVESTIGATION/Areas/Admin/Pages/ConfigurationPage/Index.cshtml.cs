using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Options;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ConfigurationViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Services;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.ConfigurationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public IndexModel(
            AkdemicContext context,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _context = context;
            _storageCredentials = storageCredentials;
        }

        [BindProperty]
        public ConfigurationViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var values = await _context.Configurations.ToDictionaryAsync(x => x.Key, x => x.Value);

            Input = new ConfigurationViewModel
            {
                RulesArticleScientific = GetConfigurationValue(values, ConfigurationConstants.TEACHERINVESTIGATION.RULES_ARTICLESCIENTIFIC),
                HasSinglePostulantRestriction = bool.Parse(GetConfigurationValue(values, ConfigurationConstants.TEACHERINVESTIGATION.HAS_SINGLEPOSTULANT_RESTRICTION)),
                AllowRegistrationRequest = bool.Parse(GetConfigurationValue(values, ConfigurationConstants.TEACHERINVESTIGATION.ALLOW_REGISTRATION_REQUEST)),
                PublicationTermsAndCondition = GetConfigurationValue(values, ConfigurationConstants.TEACHERINVESTIGATION.PUBLICATION_TERMS_AND_CONDITION)
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var storage = new CloudStorageService(_storageCredentials); 

            if (Input.File != null)
            {
                string fileUrl = await storage.UploadFile(Input.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                Path.GetExtension(Input.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                Input.RulesArticleScientific = fileUrl;

                await UpdateConfigurationValue(ConfigurationConstants.TEACHERINVESTIGATION.RULES_ARTICLESCIENTIFIC, Input.RulesArticleScientific.ToString());
            }

            await UpdateConfigurationValue(ConfigurationConstants.TEACHERINVESTIGATION.HAS_SINGLEPOSTULANT_RESTRICTION, Input.HasSinglePostulantRestriction.ToString());
            await UpdateConfigurationValue(ConfigurationConstants.TEACHERINVESTIGATION.ALLOW_REGISTRATION_REQUEST, Input.AllowRegistrationRequest.ToString());
            await UpdateConfigurationValue(ConfigurationConstants.TEACHERINVESTIGATION.PUBLICATION_TERMS_AND_CONDITION, Input.PublicationTermsAndCondition.ToString());

            return new OkResult();
        }

        private async Task UpdateConfigurationValue(string key, string value)
        {
            if (value != null)
            {
                var configuration = await _context.Configurations.Where(x => x.Key == key).FirstOrDefaultAsync();

                if (configuration != null)
                {
                    configuration.Value = value;
                }
                else
                {
                    configuration = new DOMAIN.Entities.General.Configuration
                    {
                        Key = key,
                        Value = value
                    };
                    await _context.Configurations.AddAsync(configuration);
                }
                await _context.SaveChangesAsync();
            }
        }
        /// <summary>
        /// Retorna el valor de una variable de configuraci?n
        /// </summary>
        /// <param name="list">Lista de valores actuales</param>
        /// <param name="key">Variable de configuraci?n a buscar</param>
        /// <returns>Valor de la variable</returns>
        private string GetConfigurationValue(Dictionary<string, string> list, string key)
        {
            return list.ContainsKey(key) ? list[key] :

                ConfigurationConstants.TEACHERINVESTIGATION.DEFAULT_VALUES.ContainsKey(key) ?
                ConfigurationConstants.TEACHERINVESTIGATION.DEFAULT_VALUES[key] : null;
        }

    }
}

