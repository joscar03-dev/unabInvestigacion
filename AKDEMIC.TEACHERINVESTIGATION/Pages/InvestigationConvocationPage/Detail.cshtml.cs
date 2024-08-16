using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels;
using AKDEMIC.TEACHERINVESTIGATION.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Pages.InvestigationConvocationPage
{
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpClientFactory _clientFactory;

        public DetailModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IHttpClientFactory clientFactory
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _clientFactory = clientFactory;
        }


        public InvestigationConvocationDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid investigationConvocationId)
        {
            var convocation = await _context.InvestigationConvocations
                .Where(x => x.Id == investigationConvocationId)
                .Select(x => new InvestigationConvocationDetailViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    InscriptionEndDate = x.InscriptionEndDate.ToLocalDateTimeFormat(),
                    InscriptionStartDate = x.InscriptionStartDate.ToLocalDateTimeFormat(),
                    Description = x.Description,
                    PicturePath = x.PicturePath,
                    UserSigned = "",
                    SignedUp = false
                })
                .FirstOrDefaultAsync();

            if (convocation == null)
            {
                return RedirectToPage("/Pages/Index");
            }

            bool userSignedUp = false;
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);

                if (user != null)
                {
                    userSignedUp = await _context.InvestigationConvocationPostulants
                        .AnyAsync(x => x.InvestigationConvocationId == convocation.Id && x.UserId == user.Id);
                    convocation.UserSigned = $"{user.UserName}-{user.FullName}";
                }
            }

            convocation.SignedUp = userSignedUp;


            Input = convocation;

            return Page();
        }

        public async Task<IActionResult> OnPostRegisterToConvocationAsync(InvestigationConvocationPostulantViewModel viewModel)
        {
            var hasSinglePostulantRestriction = await _context.Configurations.Where(x => x.Key == ConfigurationConstants.TEACHERINVESTIGATION.HAS_SINGLEPOSTULANT_RESTRICTION).FirstOrDefaultAsync();

            if (hasSinglePostulantRestriction == null)
            {
                hasSinglePostulantRestriction = new DOMAIN.Entities.General.Configuration
                {
                    Key = ConfigurationConstants.TEACHERINVESTIGATION.HAS_SINGLEPOSTULANT_RESTRICTION,
                    Value = ConfigurationConstants.TEACHERINVESTIGATION.DEFAULT_VALUES[ConfigurationConstants.TEACHERINVESTIGATION.HAS_SINGLEPOSTULANT_RESTRICTION]
                };
                await _context.Configurations.AddAsync(hasSinglePostulantRestriction);
                await _context.SaveChangesAsync();
            }

            bool hasSinglePostulantRestrictionValue = bool.Parse(hasSinglePostulantRestriction.Value);

            var today = DateTime.UtcNow;

            var convocation = await _context.InvestigationConvocations
                .Where(x => x.Id == viewModel.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            if (convocation == null)
                return new BadRequestObjectResult("Ha sucedido un error");

            if (!(today >= convocation.InscriptionStartDate && today <= convocation.InscriptionEndDate))
            {
                return new BadRequestObjectResult("Se encuentra fuera del rango de inscripción a la convocatoria");
            }

            if (User?.Identity.IsAuthenticated == false)
            {
                return new BadRequestObjectResult("Debe logear para poder postular");
            }

            var user = await _userManager.GetUserAsync(User);

            //Tiene que ser investigador
            var isInRole = await _userManager.IsInRoleAsync(user, GeneralConstants.ROLES.RESEARCHERS);

            if (!isInRole)
            {
                return new BadRequestObjectResult("La convocatorias estan disponibles solo para investigadores");
            }

            //Que no haya postulado ya a esta convocatoria
            var isPostulant = await _context.InvestigationConvocationPostulants
                .AnyAsync(x => x.UserId == user.Id && x.InvestigationConvocationId == convocation.Id);

            if (isPostulant)
            {
                return new BadRequestObjectResult("ERROR: Ya postulo a esta convocatoria");
            }

            if (hasSinglePostulantRestrictionValue)
            {
                //Postulaciones que son la actual, y esten vigentes, ademas que no haya sido rechazado
                var userCurrentPostulations = await _context.InvestigationConvocationPostulants
                    .Where(x => x.UserId == user.Id &&
                        x.InvestigationConvocation.EndDate > today && x.InvestigationConvocation.StartDate < today &&
                        x.InvestigationConvocationId != convocation.Id &&
                        x.ReviewState != CORE.Constants.Systems.TeacherInvestigationConstants.ConvocationPostulant.ReviewState.DECLINED &&
                        x.ProjectState != CORE.Constants.Systems.TeacherInvestigationConstants.ConvocationPostulant.ProjectState.DECLINED)
                    .Select(x => new
                    {
                        x.Id,
                        x.UserId,
                        x.User.UserName,
                        x.User.FullName,
                        x.InvestigationConvocation.Code,
                        x.InvestigationConvocation.Name,
                        x.ProjectState,
                        x.ReviewState
                    })
                    .ToListAsync();

                if (userCurrentPostulations.Count > 0)
                {
                    return new BadRequestObjectResult("ERROR: El investigador ya esta inscrito en una convocatoria vigente");
                }
            }


            var investigationConvocationPostulant = new InvestigationConvocationPostulant
            {
                InvestigationConvocationId = convocation.Id,
                UserId = user.Id
            };

            await _context.InvestigationConvocationPostulants.AddAsync(investigationConvocationPostulant);
            await _context.SaveChangesAsync();

            return new OkObjectResult(investigationConvocationPostulant.Id);
        }
    }
}