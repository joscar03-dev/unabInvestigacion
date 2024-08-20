using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Options;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.INFRASTRUCTURE.Factories;
using AKDEMIC.TEACHERINVESTIGATION.Configuration;
using AKDEMIC.TEACHERINVESTIGATION.Helpers;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using AKDEMIC.CORE.Extensions;
using System.Linq;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using IdentityModel;
using static AKDEMIC.CORE.Constants.GeneralConstants;
using System.Collections.Generic;
using AKDEMIC.CORE.Services;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;


namespace AKDEMIC.TEACHERINVESTIGATION
{
    public class Startup
    {
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AkdemicContext>(options =>
            {
                switch (GeneralConstants.Database.DATABASE)
                {
                    case DataBaseConstants.MYSQL:
                        options.UseMySql(Configuration.GetConnectionString("MySqlDefaultConnection"), new MySqlServerVersion(DataBaseConstants.Versions.MySql.VALUES[DataBaseConstants.Versions.MySql.V8021]), mySqlOptions => mySqlOptions.EnableRetryOnFailure());
                        break;
                    case DataBaseConstants.SQL:
                        options.UseSqlServer(Configuration.GetConnectionString("SqlDefaultConnection"), sqlServerOptions => sqlServerOptions.EnableRetryOnFailure());
                        break;
                }
            });

            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 5;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AkdemicContext>()
                .AddDefaultTokenProviders();


            #region OpenIdConnect

            if (GeneralConstants.Authentication.SSO_ENABLED)
            {
                services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = /*OpenIdConnectDefaults.AuthenticationScheme*/"oidc";
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                    {
                        options.SlidingExpiration = true;
                        options.AccessDeniedPath = "/acceso-denegado";
                    })
                    .AddOpenIdConnect(/*OpenIdConnectDefaults.AuthenticationScheme*/"oidc", options =>
                    {
                        options.Authority = GeneralConstants.GetAuthority(CurrentEnvironment.IsDevelopment());

                        options.RequireHttpsMetadata = false;
                        options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;

                        options.ClientId = "teacherinvestigation";
                        options.ClientSecret = "secret";
                        options.ResponseType = OpenIdConnectResponseType.Code;

                        options.Scope.Clear();
                        options.Scope.Add("openid");
                        options.Scope.Add("profile");
                        options.Scope.Add("roles");

                        // keeps id_token smaller
                        options.GetClaimsFromUserInfoEndpoint = true;
                        options.SaveTokens = true;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            NameClaimType = ClaimTypes.Name,
                            RoleClaimType = ClaimTypes.Role,
                        };

                        options.RemoteAuthenticationTimeout = TimeSpan.FromHours(1);
                        options.Events.OnRemoteFailure = RemoteAuthFail;

                        options.Events.OnUserInformationReceived = UserInformationReceivedEvent;
                    });

                // add automatic token management
                services.AddAccessTokenManagement();
            }

            #endregion OpenIdConnect

            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ClaimsPrincipalFactory>();


            if (!GeneralConstants.Authentication.SSO_ENABLED)
            {
                services.AddCookieSettings();
            }
            else
            {
                services.ConfigureNonBreakingSameSiteCookies();
            }

            services.Configure<CloudStorageCredentials>(Configuration.GetSection("AzureStorageCredentials"));
            services.AddCoreServices();

            #region API
            services.AddHttpClient("akdemic", o =>
            {
                o.BaseAddress = new System.Uri(GeneralConstants.GetApplicationRoute(GeneralConstants.Solution.WebApi, CurrentEnvironment.IsDevelopment()));
            });
            #endregion


            services.AddRazorPages();

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddHttpContextAccessor();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IQRService, QRService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
                app.UseExceptionHandler("/error/500");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            if (GeneralConstants.Authentication.SSO_ENABLED)
            {
                var forwardOptions = new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.All,
                    //RequireHeaderSymmetry = fale
                };
                forwardOptions.KnownNetworks.Clear();
                forwardOptions.KnownProxies.Clear();
                app.UseForwardedHeaders(forwardOptions);
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "clientapp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }

        private Task RemoteAuthFail(RemoteFailureContext context) { context.Response.Redirect("/"); context.HandleResponse(); return Task.CompletedTask; }

        private async Task UserInformationReceivedEvent(UserInformationReceivedContext context)
        {
            var _dbContext = context.HttpContext.RequestServices.GetRequiredService<AkdemicContext>();
            var userName = context.Principal.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault();
            
            
            var userRoles = await _dbContext.UserRoles.Where(x => x.User.UserName == userName).Select(x => x.Role.Name).ToListAsync();

            var identity = context.Principal.Identities.First();

            var claimsToRemove = context.Principal.Claims.Where(x => x.Type == JwtClaimTypes.Role || x.Type == "RolePriorityName").ToList();

            foreach (var _claim in claimsToRemove)
            {
                identity.RemoveClaim(_claim);
            }

            var claims = new List<Claim>();

            claims.AddRange(userRoles.Select(x => new Claim(JwtClaimTypes.Role, x)));

            if (userRoles.Count > 0)
            {
                var rolePriotiry = await _dbContext.UserRoles
                    .Where(x => x.User.UserName == userName)
                    .OrderByDescending(x => x.Role.Priority)
                    .Select(x => x.Role.Name).FirstOrDefaultAsync();

                claims.Add(new Claim("RolePriorityName", rolePriotiry));
            }
            else
            {
                claims.Add(new Claim("RolePriorityName", ""));
            }

            identity.AddClaims(claims);
        }
    }
}
