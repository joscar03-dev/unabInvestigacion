using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.General.Implementations;
using AKDEMIC.CORE.Services.General.Interfaces;
using AKDEMIC.CORE.Services.TeacherInvestigation.Implementations;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace AKDEMIC.TEACHERINVESTIGATION.Configuration
{
    public static class ConfigureCoreServices
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddTransient<IDataTablesService, DataTablesService>();
            services.AddTransient<ISelect2Service, Select2Service>();

            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IIncubatorConvocationFileService, IncubatorConvocationFileService>();
            services.AddScoped<IIncubatorConvocationAnnexService, IncubatorConvocationAnnexService>();
            services.AddScoped<IInvestigationConvocationFileService, InvestigationConvocationFileService>();
            services.AddScoped<IInvestigationConvocationService, InvestigationConvocationService>();
            services.AddScoped<IInvestigationConvocationInquiryService, InvestigationConvocationInquiryService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();

            services.AddTransient<ICloudStorageService, CloudStorageService>();
            services.AddTransient<IEmailSenderService, EmailSenderService>();
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddScoped<ITextSharpService, TextSharpService>();
            services.AddTransient<IRandomPasswordService, RandomPasswordService>();
            return services;
        }
    }
}
