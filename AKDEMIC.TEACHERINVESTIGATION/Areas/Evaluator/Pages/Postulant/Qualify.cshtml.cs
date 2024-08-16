using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.IncubatorPostulationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.Pages.Postulant
{
    [Authorize(Roles = GeneralConstants.ROLES.EXTERNAL_EVALUATOR)]
    public class QualifyModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QualifyModel(
            IDataTablesService dataTablesService,
            AkdemicContext context,
            UserManager<ApplicationUser> userManager
            )
        {
            _dataTablesService = dataTablesService;
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public IncubatorPostulantViewModel Postulant { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid investigationConvocationPostulantId)
        {
            var user = await _userManager.GetUserAsync(User);

            Postulant = await _context.InvestigationConvocationPostulants.Where(x => x.Id == investigationConvocationPostulantId)
                .Select(x=> new IncubatorPostulantViewModel
                {
                    Id = x.Id,
                    ConvocationCode = x.InvestigationConvocation.Code,
                    ConvocationId = x.InvestigationConvocationId,
                    ConvocationName = x.InvestigationConvocation.Name,
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    ReviewState = x.ReviewState,
                    ReviewStateText = TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.ConvocationPostulant.ReviewState.VALUES[x.ReviewState] : ""
                })
                .FirstOrDefaultAsync();

            var isEvaluator = await _context.InvestigationConvocationEvaluators
                .AnyAsync(x => x.InvestigationConvocationId == Postulant.ConvocationId && x.UserId == user.Id);

            if (!isEvaluator)
                return Redirect("/evaluador/postulantes-investigacion");

            if (Postulant.ReviewState != TeacherInvestigationConstants.ConvocationPostulant.ReviewState.ADMITTED)
                return Redirect("/evaluador/postulantes-investigacion");
            
            var postulantQualifications = await _context.PostulantRubricQualifications
                .Where(x => x.EvaluatorId == user.Id && x.InvestigationConvocationPostulantId == investigationConvocationPostulantId)
                .ToListAsync();

            var sections = await _context.InvestigationRubricSections
                .Where(x => x.InvestigationConvocationId == Postulant.ConvocationId)
                .ToListAsync();

            var criterions = await _context.InvestigationRubricCriterions
                .Where(x => x.InvestigationRubricSection.InvestigationConvocationId == Postulant.ConvocationId)
                .Select(x => new 
                {
                    x.Id,
                    x.InvestigationRubricSectionId,
                    x.Description,
                    x.Name,
                    InvestigationRubricLevels = x.InvestigationRubricLevels
                        .Select(y => new 
                        {
                            y.Id,
                            y.Description,
                            y.Score
                        })
                        .ToList()
                })
                .ToListAsync();

            Postulant.Sections = sections
                .Select(x => new IncubatorRubricSectionViewModel
                {
                    Title = x.Title,
                    MaxSectionScore = x.MaxSectionScore,
                    RubricCriterions = criterions
                        .Where(y => y.InvestigationRubricSectionId == x.Id)
                        .Select(y => new IncubatorRubricCriterionViewModel
                        {
                            Id = y.Id,
                            Description = y.Description,
                            MaxScore = y.InvestigationRubricLevels.Count() == 0 ? 0.0m : y.InvestigationRubricLevels.DefaultIfEmpty().Max(z => z.Score),
                            Name = y.Name,
                            Qualification = postulantQualifications.Any(z => z.InvestigationRubricCriterionId == y.Id) ? postulantQualifications.Where(z => z.InvestigationRubricCriterionId == y.Id).Select(z => z.Value).FirstOrDefault() : null,
                            Levels = y.InvestigationRubricLevels
                                .OrderByDescending(z => z.Score)
                                .Select(z => new IncubatorRubricLevelViewModel
                                {
                                    Description = z.Description,
                                    Id = z.Id,
                                    Score = z.Score
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostQualifyAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var isEvaluator = await _context.InvestigationConvocationEvaluators.AnyAsync(x => x.InvestigationConvocationId == Postulant.ConvocationId && x.UserId == user.Id);

            var postulant = await _context.InvestigationConvocationPostulants.Where(x => x.Id == Postulant.Id).FirstOrDefaultAsync();

            if (!isEvaluator)
                return new BadRequestObjectResult("No tiene permiso para calificar al postulante.");

            var postulantQualifications = await _context.PostulantRubricQualifications.Where(x => x.EvaluatorId == user.Id && x.InvestigationConvocationPostulantId == postulant.Id).ToListAsync();

            if (postulantQualifications.Any())
                return new BadRequestObjectResult("El postulante ya ha sido calificado.");

            if (postulant.ReviewState != TeacherInvestigationConstants.ConvocationPostulant.ReviewState.ADMITTED)
                return new BadRequestObjectResult("El postulante debe ser admitido por el comité técnico para ser calificado");

            int added = 0;
            foreach (var section in Postulant.Sections)
            {
                var rubricQualification = section.RubricCriterions
                    .Select(x => new PostulantRubricQualification
                    {
                        EvaluatorId = user.Id,
                        InvestigationConvocationPostulantId = postulant.Id,
                        InvestigationRubricCriterionId = x.Id,
                        Value = x.Qualification.Value
                    })
                    .ToList();

                await _context.PostulantRubricQualifications.AddRangeAsync(rubricQualification);
                added++;
            }

            if (added > 0)
            {
                await _context.SaveChangesAsync();
            }

            return new OkResult();
        }
    }
}
