using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System;
using AKDEMIC.CORE.Constants;
using Microsoft.AspNetCore.Authorization;
using AKDEMIC.CORE.Services;
using AKDEMIC.INFRASTRUCTURE.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT)]
    public class IncubatorRubricModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly AkdemicContext _context;

        public IncubatorRubricModel(IDataTablesService dataTablesService,
            AkdemicContext context)
        {
            _dataTablesService = dataTablesService;
            _context = context;
        }

        [BindProperty]
        public IncubatorConvocationDetailViewModel IncubatorConvocation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorConvocationId)
        {
            var incubatorConvocation = await _context.IncubatorConvocations
                .Where(x => x.Id == incubatorConvocationId)
                .FirstOrDefaultAsync();

            if (incubatorConvocation == null)
                return RedirectToPage("/");

            var hasRubricQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorConvocation.Id);

            IncubatorConvocation = new IncubatorConvocationDetailViewModel
            {
                Id = incubatorConvocation.Id,
                Code = incubatorConvocation.Code,
                Name = incubatorConvocation.Name,
                HasRubricQualifications = hasRubricQualifications
            };

            return Page();
        }

        public async Task<IActionResult> OnGetRubricPartialViewAsync(Guid incubatorConvocationId)
        {
            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
               .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorConvocationId);

            var model = new IncubatorRubricViewModel
            {
                HasQualifications = hasQualifications
            };

            var data = await _context.IncubatorRubricSections
                .Include(x => x.IncubatorRubricCriterions)
                    .ThenInclude(x => x.IncubatorRubricLevels)
                .Where(x => x.IncubatorConvocationId == incubatorConvocationId)
                .ToListAsync();

            var sections = data
                .OrderBy(x => x.CreatedAt)
                .Select(x => new IncubatorRubricSectionViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    MaxSectionScore = x.MaxSectionScore,
                    ConvocationId = x.IncubatorConvocationId,
                    RubricCriterions = x.IncubatorRubricCriterions
                        .Select(y => new IncubatorRubricCriterionViewModel
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Description = y.Description,
                            RubricSectionId = y.IncubatorRubricSectionId,
                            Levels = y.IncubatorRubricLevels
                                .OrderByDescending(z => z.Score)
                                .Select(z => new IncubatorRubricLevelViewModel
                                {
                                    Id = z.Id,
                                    Description = z.Description,
                                    Score = z.Score,
                                    RubricCriterionId = z.IncubatorRubricCriterionId
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToList();

            model.RubricSections = sections;

            return Partial("Partials/_RubricPartialView", model);
        }

        #region SECCIONES

        public async Task<IActionResult> OnPostAddSectionAsync(IncubatorRubricSectionViewModel model)
        {
            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
               .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == model.ConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var incubatorRubricSection = new IncubatorRubricSection
            {
                Title = model.Title,
                MaxSectionScore = model.MaxSectionScore,
                IncubatorConvocationId = model.ConvocationId,
            };

            await _context.IncubatorRubricSections.AddAsync(incubatorRubricSection);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditSectionAsync(IncubatorRubricSectionViewModel model)
        {
            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
               .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == model.ConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var incubatorRubricSection = await _context.IncubatorRubricSections.Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();

            if (incubatorRubricSection == null)
                return new BadRequestResult();

            incubatorRubricSection.Title = model.Title;
            incubatorRubricSection.MaxSectionScore = model.MaxSectionScore;
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteSectionAsync(Guid id)
        {
            var incubatorRubricSection = await _context.IncubatorRubricSections.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (incubatorRubricSection == null)
                return new BadRequestResult();

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricSection.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var criterions = await _context.IncubatorRubricCriterions
                .Where(x => x.IncubatorRubricSectionId == incubatorRubricSection.Id)
                .ToListAsync();

            var levels = await _context.IncubatorRubricLevels
                .Where(x => x.IncubatorRubricCriterion.IncubatorRubricSectionId == incubatorRubricSection.Id)
                .ToListAsync();

            if (levels.Count > 0)
            {
                _context.IncubatorRubricLevels.RemoveRange(levels);
            }

            if (criterions.Count > 0)
            {
                _context.IncubatorRubricCriterions.RemoveRange(criterions);
            }

            _context.IncubatorRubricSections.Remove(incubatorRubricSection);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion


        #region CRITERIOS

        public async Task<IActionResult> OnPostAddCriterionAsync(IncubatorRubricCriterionViewModel model)
        {

            var incubatorRubricSection = await _context.IncubatorRubricSections
                .Where(x => x.Id == model.RubricSectionId)
                .FirstOrDefaultAsync();

            if (incubatorRubricSection == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricSection.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var entity = new IncubatorRubricCriterion
            {
                Name = model.Name,
                Description = model.Description,
                IncubatorRubricSectionId = incubatorRubricSection.Id,
            };

            await _context.IncubatorRubricCriterions.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnPostEditCriterionAsync(IncubatorRubricCriterionViewModel model)
        {

            var incubatorRubricSection = await _context.IncubatorRubricSections
                .Where(x => x.Id == model.RubricSectionId)
                .FirstOrDefaultAsync();

            if (incubatorRubricSection == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricSection.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var incubatorRubricCriterion = await _context.IncubatorRubricCriterions
                .Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();

            if (incubatorRubricCriterion == null)
                return new BadRequestResult();

            incubatorRubricCriterion.Description = model.Description;
            incubatorRubricCriterion.Name = model.Name;
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteCriterionAsync(Guid id)
        {
            var incubatorRubricCriterion = await _context.IncubatorRubricCriterions.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (incubatorRubricCriterion == null)
                return new BadRequestResult();

            var incubatorRubricSection = await _context.IncubatorRubricSections
                .Where(x => x.Id == incubatorRubricCriterion.IncubatorRubricSectionId)
                .FirstOrDefaultAsync();

            if (incubatorRubricSection == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricSection.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var levels = await _context.IncubatorRubricLevels
                .Where(x => x.IncubatorRubricCriterionId == incubatorRubricCriterion.Id)
                .ToListAsync();

            if (levels.Count > 0)
            {
                _context.IncubatorRubricLevels.RemoveRange(levels);
            }
            _context.IncubatorRubricCriterions.Remove(incubatorRubricCriterion);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion

        #region NIVELES DESEMPEÑO

        public async Task<IActionResult> OnPostAddLevelAsync(IncubatorRubricLevelViewModel model)
        {
            var incubatorRubricCriterion = await _context.IncubatorRubricCriterions
                .Where(x => x.Id == model.RubricCriterionId)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorRubricSectionId,
                    x.IncubatorRubricSection.MaxSectionScore,
                    x.IncubatorRubricSection.IncubatorConvocationId
                })
                .FirstOrDefaultAsync();

            if (incubatorRubricCriterion == null)
                return new BadRequestResult();

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricCriterion.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var levels = await _context.IncubatorRubricLevels
                .Where(x => x.IncubatorRubricCriterion.IncubatorRubricSectionId == incubatorRubricCriterion.IncubatorRubricSectionId &&
                    x.IncubatorRubricCriterionId != model.RubricCriterionId)
                .GroupBy(x => new { x.IncubatorRubricCriterionId })
                .Select(x => new
                {
                    RubricCriterionId = x.Key.IncubatorRubricCriterionId,
                    MaxLevel = x.Max(y => y.Score)
                })
                .ToListAsync();

            var totalMaxScore = levels.Select(x => x.MaxLevel).DefaultIfEmpty().Sum();


            if (totalMaxScore + model.Score > incubatorRubricCriterion.MaxSectionScore)
            {
                return new BadRequestObjectResult($"El puntaje máximo de toda la seccion no puede permitir mas de {incubatorRubricCriterion.MaxSectionScore} puntos en total");
            }


            var incubatorRubricLevel = new IncubatorRubricLevel
            {
                Description = model.Description,
                IncubatorRubricCriterionId = model.RubricCriterionId,
                Score = model.Score
            };

            await _context.IncubatorRubricLevels.AddAsync(incubatorRubricLevel);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditLevelAsync(IncubatorRubricLevelViewModel model)
        {
            var incubatorRubricLevel = await _context.IncubatorRubricLevels.Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();
            if (incubatorRubricLevel == null)
                return new BadRequestObjectResult("Sucedio un error");

            var incubatorRubricCriterion = await _context.IncubatorRubricCriterions
                .Where(x => x.Id == incubatorRubricLevel.IncubatorRubricCriterionId)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorRubricSectionId,
                    x.IncubatorRubricSection.MaxSectionScore,
                    x.IncubatorRubricSection.IncubatorConvocationId,
                })
                .FirstOrDefaultAsync();

            if (incubatorRubricCriterion == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricCriterion.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var levels = await _context.IncubatorRubricLevels
                .Where(x => x.IncubatorRubricCriterion.IncubatorRubricSectionId == incubatorRubricCriterion.IncubatorRubricSectionId &&
                    x.IncubatorRubricCriterionId != incubatorRubricLevel.IncubatorRubricCriterionId)
                .GroupBy(x => new { x.IncubatorRubricCriterionId })
                .Select(x => new
                {
                    RubricCriterionId = x.Key.IncubatorRubricCriterionId,
                    MaxLevel = x.Max(y => y.Score)
                })
                .ToListAsync();

            var totalMaxScore = levels.Select(x => x.MaxLevel).DefaultIfEmpty().Sum();


            if (totalMaxScore + model.Score > incubatorRubricCriterion.MaxSectionScore)
            {
                return new BadRequestObjectResult($"El puntaje máximo de toda la seccion no puede permitir mas de {incubatorRubricCriterion.MaxSectionScore} puntos en total");
            }

            incubatorRubricLevel.Description = model.Description;
            incubatorRubricLevel.Score = model.Score;

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteLevelAsync(Guid id)
        {
            var incubatorRubricLevel = await _context.IncubatorRubricLevels.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (incubatorRubricLevel == null)
                return new BadRequestResult();

            var incubatorRubricCriterion = await _context.IncubatorRubricCriterions
                .Where(x => x.Id == incubatorRubricLevel.IncubatorRubricCriterionId)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorRubricSectionId,
                    x.IncubatorRubricSection.MaxSectionScore,
                    x.IncubatorRubricSection.IncubatorConvocationId,
                })
                .FirstOrDefaultAsync();

            if (incubatorRubricCriterion == null)
                return new BadRequestResult();

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
                .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorRubricCriterion.IncubatorConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            _context.IncubatorRubricLevels.Remove(incubatorRubricLevel);
            await _context.SaveChangesAsync();

            return new OkResult();
        }


        #endregion

        public async Task<IActionResult> OnPostImportAsync(IncubatorRubricImportViewModel model)
        {
            var incubatorConvocation = await _context.IncubatorConvocations
                .Where(x => x.Id == model.IncubatorConvocationId)
                .FirstOrDefaultAsync();

            var incubatorConvocationToExport = await _context.IncubatorConvocations
                .Where(x => x.Code == model.ConvocationCodeToExport)
                .FirstOrDefaultAsync();

            if (incubatorConvocation == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (incubatorConvocationToExport == null)
                return new BadRequestObjectResult("No se encontró ninguna convocatoria con este código");

            if (incubatorConvocationToExport.Id == incubatorConvocation.Id)
                return new BadRequestObjectResult("No puedes exportar la misma convocatoria");

            var hasRubricSection = await _context.IncubatorRubricSections
                .AnyAsync(x => x.IncubatorConvocationId == incubatorConvocation.Id);

            var hasQualifications = await _context.IncubatorPostulantRubricQualifications
               .AnyAsync(x => x.IncubatorPostulation.IncubatorConvocationId == incubatorConvocation.Id);

            if (hasRubricSection)
                return new BadRequestObjectResult("La convocatoría actual ya presenta una rubrica");

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");


            //Si hemos pasado las validaciones copiaremos la rubrica de la convocatoria que exportaremos
            var rubricSections = await _context.IncubatorRubricSections
                .Where(x => x.IncubatorConvocationId == incubatorConvocationToExport.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.IncubatorConvocationId,
                    x.MaxSectionScore,
                    criterions = x.IncubatorRubricCriterions
                        .Where(y => y.IncubatorRubricSectionId == x.Id)
                        .Select(y => new
                        {
                            y.Id,
                            y.IncubatorRubricSectionId,
                            y.Name,
                            y.Description,
                            levels = y.IncubatorRubricLevels
                                .Where(z => z.IncubatorRubricCriterionId == y.Id)
                                .Select(z => new
                                {
                                    z.Id,
                                    z.Score,
                                    z.Description,
                                    z.IncubatorRubricCriterionId
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            //Recorremos las secciones, criterios y niveles
            foreach (var sectionItem in rubricSections)
            {
                var incubatorRubricSection = new IncubatorRubricSection
                {
                    Title = sectionItem.Title,
                    MaxSectionScore = sectionItem.MaxSectionScore,
                    IncubatorConvocationId = incubatorConvocation.Id
                };
                await _context.IncubatorRubricSections.AddAsync(incubatorRubricSection);

                foreach (var criterionItem in sectionItem.criterions)
                {
                    var incubatorRubricCriterion = new IncubatorRubricCriterion
                    {
                        Name = criterionItem.Name,
                        Description = criterionItem.Description,
                        IncubatorRubricSectionId = incubatorRubricSection.Id
                    };
                    await _context.IncubatorRubricCriterions.AddAsync(incubatorRubricCriterion);

                    foreach (var levelItem in criterionItem.levels)
                    {
                        var incubatorRubricLevel = new IncubatorRubricLevel
                        {
                            Score = levelItem.Score,
                            Description = levelItem.Description,
                            IncubatorRubricCriterionId = incubatorRubricCriterion.Id
                        };
                        await _context.IncubatorRubricLevels.AddAsync(incubatorRubricLevel);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
