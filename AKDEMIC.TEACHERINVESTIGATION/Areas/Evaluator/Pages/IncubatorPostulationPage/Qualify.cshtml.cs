using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.ViewModels.PostulantViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Evaluator.Pages.IncubatorPostulationPage
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
        public PostulantViewModel Postulant { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorPostulationId)
        {
            var user = await _userManager.GetUserAsync(User);

            Postulant = await _context.IncubatorPostulations.Where(x => x.Id == incubatorPostulationId)
                .Select(x => new PostulantViewModel
                {
                    Id = x.Id,
                    ConvocationCode = x.IncubatorConvocation.Code,
                    ConvocationId = x.IncubatorConvocationId,
                    ConvocationName = x.IncubatorConvocation.Name,
                    FullName = x.User.FullName,
                    UserName = x.User.UserName,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    ReviewState = x.ReviewState,
                    ReviewStateText = TeacherInvestigationConstants.IncubatorPostulation.ReviewState.VALUES.ContainsKey(x.ReviewState) ?
                        TeacherInvestigationConstants.IncubatorPostulation.ReviewState.VALUES[x.ReviewState] : ""
                })
                .FirstOrDefaultAsync();

            var isEvaluator = await _context.IncubatorConvocationEvaluators
                .AnyAsync(x => x.IncubatorConvocationId == Postulant.ConvocationId && x.UserId == user.Id);

            if (!isEvaluator)
                return Redirect("/evaluador/postulantes-incubadora");

            if (Postulant.ReviewState != TeacherInvestigationConstants.IncubatorPostulation.ReviewState.PENDING)
                return Redirect("/evaluador/postulantes-incubadora");

            var postulantQualifications = await _context.IncubatorPostulantRubricQualifications
                .Where(x => x.EvaluatorId == user.Id && x.IncubatorPostulationId == incubatorPostulationId)
                .ToListAsync();

            var sections = await _context.IncubatorRubricSections
                .Where(x => x.IncubatorConvocationId == Postulant.ConvocationId)
                .ToListAsync();

            var criterions = await _context.IncubatorRubricCriterions
                .Where(x => x.IncubatorRubricSection.IncubatorConvocationId == Postulant.ConvocationId)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorRubricSectionId,
                    x.Description,
                    x.Name,
                    IncubatorRubricLevels = x.IncubatorRubricLevels
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
                .Select(x => new RubricSectionViewModel
                {
                    Title = x.Title,
                    MaxSectionScore = x.MaxSectionScore,
                    RubricCriterions = criterions
                        .Where(y => y.IncubatorRubricSectionId == x.Id)
                        .Select(y => new RubricCriterionViewModel
                        {
                            Id = y.Id,
                            Description = y.Description,
                            MaxScore = y.IncubatorRubricLevels.Count() == 0 ? 0.0m : y.IncubatorRubricLevels.DefaultIfEmpty().Max(z => z.Score),
                            Name = y.Name,
                            Qualification = postulantQualifications.Any(z => z.IncubatorRubricCriterionId == y.Id) ? postulantQualifications.Where(z => z.IncubatorRubricCriterionId == y.Id).Select(z => z.Value).FirstOrDefault() : null,
                            Levels = y.IncubatorRubricLevels
                                .OrderByDescending(z => z.Score)
                                .Select(z => new RubricLevelViewModel
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
            var isEvaluator = await _context.IncubatorConvocationEvaluators.AnyAsync(x => x.IncubatorConvocationId == Postulant.ConvocationId && x.UserId == user.Id);

            var postulant = await _context.IncubatorPostulations.Where(x => x.Id == Postulant.Id).FirstOrDefaultAsync();

            if (!isEvaluator)
                return new BadRequestObjectResult("No tiene permiso para calificar al postulante.");

            var postulantQualifications = await _context.IncubatorPostulantRubricQualifications.Where(x => x.EvaluatorId == user.Id && x.IncubatorPostulationId == postulant.Id).ToListAsync();

            if (postulantQualifications.Any())
                return new BadRequestObjectResult("El postulante ya ha sido calificado.");

            if (postulant.ReviewState != TeacherInvestigationConstants.ConvocationPostulant.ReviewState.PENDING)
                return new BadRequestObjectResult("El postulante debe ser admitido por el comité técnico para ser calificado");

            int added = 0;
            foreach (var section in Postulant.Sections)
            {
                var rubricQualification = section.RubricCriterions
                    .Select(x => new IncubatorPostulantRubricQualification
                    {
                        EvaluatorId = user.Id,
                        IncubatorPostulationId = postulant.Id,
                        IncubatorRubricCriterionId = x.Id,
                        Value = x.Qualification.Value
                    })
                    .ToList();

                await _context.IncubatorPostulantRubricQualifications.AddRangeAsync(rubricQualification);
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
