using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class ConvocationRubricModel : PageModel
    {
        private readonly AkdemicContext _context;

        public ConvocationRubricModel(
            AkdemicContext context
            )
        {
            _context = context;
        }

        [BindProperty]
        public ConvocationDetailViewModel Convocation { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid investigationConvocationId)
        {
            var investigationConvocation = await _context.InvestigationConvocations
                .Where(x => x.Id == investigationConvocationId)
                .FirstOrDefaultAsync();

            if (investigationConvocation == null)
                return RedirectToPage("/");

            var hasRubricQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationConvocation.Id);

            Convocation = new ConvocationDetailViewModel
            {
                Id = investigationConvocation.Id,
                Code = investigationConvocation.Code,
                Name = investigationConvocation.Name,
                Description = investigationConvocation.Description,
                MinScore = investigationConvocation.MinScore,
                HasRubricQualifications = hasRubricQualifications
            };

            return Page();
        }

        public async Task<IActionResult> OnGetRubricPartialViewAsync(Guid investigationConvocationId)
        {
            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationConvocationId);

            var model = new InvestigationRubricViewModel
            {
                HasQualifications = hasQualifications
            };

            var data = await _context.InvestigationRubricSections
                .Include(x => x.InvestigationRubricCriterions)
                    .ThenInclude(x => x.InvestigationRubricLevels)
                .Where(x => x.InvestigationConvocationId == investigationConvocationId)
                .ToListAsync();

            var sections = data
                .OrderBy(x => x.CreatedAt)
                .Select(x => new RubricSectionViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    MaxSectionScore = x.MaxSectionScore,
                    ConvocationId = x.InvestigationConvocationId,
                    RubricCriterions = x.InvestigationRubricCriterions
                        .Select(y => new RubricCriterionViewModel 
                        {
                            Id = y.Id,
                            Name = y.Name,
                            Description = y.Description,
                            RubricSectionId = y.InvestigationRubricSectionId,
                            Levels = y.InvestigationRubricLevels
                                .OrderByDescending(z => z.Score)
                                .Select(z => new RubricLevelViewModel 
                                {
                                    Id = z.Id,
                                    Description = z.Description,
                                    Score = z.Score,
                                    RubricCriterionId = z.InvestigationRubricCriterionId
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

        public async Task<IActionResult> OnPostAddSectionAsync(RubricSectionViewModel model)
        {
            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == model.ConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var investigationRubricSection = new InvestigationRubricSection
            {
                Title = model.Title,
                MaxSectionScore = model.MaxSectionScore,
                InvestigationConvocationId = model.ConvocationId,
            };

            await _context.InvestigationRubricSections.AddAsync(investigationRubricSection);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditSectionAsync(RubricSectionViewModel model)
        {
            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == model.ConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var investigationRubricSection = await _context.InvestigationRubricSections.Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();

            if (investigationRubricSection == null)
                return new BadRequestResult();

            investigationRubricSection.Title = model.Title;
            investigationRubricSection.MaxSectionScore = model.MaxSectionScore;
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteSectionAsync(Guid id)
        {
            var investigationRubricSection = await _context.InvestigationRubricSections.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigationRubricSection == null)
                return new BadRequestResult();

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricSection.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var criterions = await _context.InvestigationRubricCriterions
                .Where(x => x.InvestigationRubricSectionId == investigationRubricSection.Id)
                .ToListAsync();

            var levels = await _context.InvestigationRubricLevels
                .Where(x => x.InvestigationRubricCriterion.InvestigationRubricSectionId == investigationRubricSection.Id)
                .ToListAsync();

            if (levels.Count > 0)
            {
                _context.InvestigationRubricLevels.RemoveRange(levels);
            }

            if (criterions.Count > 0)
            {
                _context.InvestigationRubricCriterions.RemoveRange(criterions);
            }

            _context.InvestigationRubricSections.Remove(investigationRubricSection);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion


        #region CRITERIOS

        public async Task<IActionResult> OnPostAddCriterionAsync(RubricCriterionViewModel model)
        {

            var investigationRubricSection = await _context.InvestigationRubricSections
                .Where(x => x.Id == model.RubricSectionId)
                .FirstOrDefaultAsync();

            if (investigationRubricSection == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricSection.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var entity = new InvestigationRubricCriterion
            {
                Name = model.Name,
                Description = model.Description,
                InvestigationRubricSectionId = investigationRubricSection.Id,
            };

            await _context.InvestigationRubricCriterions.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnPostEditCriterionAsync(RubricCriterionViewModel model)
        {

            var investigationRubricSection = await _context.InvestigationRubricSections
                .Where(x => x.Id == model.RubricSectionId)
                .FirstOrDefaultAsync();

            if (investigationRubricSection == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricSection.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var investigationRubricCriterion = await _context.InvestigationRubricCriterions
                .Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();

            if (investigationRubricCriterion == null)
                return new BadRequestResult();

            investigationRubricCriterion.Description = model.Description;
            investigationRubricCriterion.Name = model.Name;
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteCriterionAsync(Guid id)
        {
            var investigationRubricCriterion = await _context.InvestigationRubricCriterions.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (investigationRubricCriterion == null)
                return new BadRequestResult();

            var investigationRubricSection = await _context.InvestigationRubricSections
                .Where(x => x.Id == investigationRubricCriterion.InvestigationRubricSectionId)
                .FirstOrDefaultAsync();

            if (investigationRubricSection == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricSection.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var levels = await _context.InvestigationRubricLevels
                .Where(x => x.InvestigationRubricCriterionId == investigationRubricCriterion.Id)
                .ToListAsync();

            if (levels.Count > 0)
            {
                _context.InvestigationRubricLevels.RemoveRange(levels);
            }
            _context.InvestigationRubricCriterions.Remove(investigationRubricCriterion);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion

        #region NIVELES DESEMPEÑO

        public async Task<IActionResult> OnPostAddLevelAsync(RubricLevelViewModel model)
        {
            var investigationRubricCriterion = await _context.InvestigationRubricCriterions
                .Where(x => x.Id == model.RubricCriterionId)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationRubricSectionId,
                    x.InvestigationRubricSection.MaxSectionScore,
                    x.InvestigationRubricSection.InvestigationConvocationId
                })
                .FirstOrDefaultAsync();

            if (investigationRubricCriterion == null)
                return new BadRequestResult();

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricCriterion.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var levels = await _context.InvestigationRubricLevels
                .Where(x => x.InvestigationRubricCriterion.InvestigationRubricSectionId == investigationRubricCriterion.InvestigationRubricSectionId && 
                    x.InvestigationRubricCriterionId != model.RubricCriterionId)
                .GroupBy(x => new { x.InvestigationRubricCriterionId })
                .Select(x => new
                {
                    RubricCriterionId = x.Key.InvestigationRubricCriterionId,
                    MaxLevel = x.Max(y => y.Score)
                })
                .ToListAsync();

            var totalMaxScore = levels.Select(x => x.MaxLevel).DefaultIfEmpty().Sum();


            if (totalMaxScore + model.Score > investigationRubricCriterion.MaxSectionScore)
            {
                return new BadRequestObjectResult($"El puntaje máximo de toda la seccion no puede permitir mas de {investigationRubricCriterion.MaxSectionScore} puntos en total");
            }


            var investigationRubricLevel = new InvestigationRubricLevel
            {
                Description = model.Description,
                InvestigationRubricCriterionId = model.RubricCriterionId,
                Score = model.Score
            };

            await _context.InvestigationRubricLevels.AddAsync(investigationRubricLevel);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditLevelAsync(RubricLevelViewModel model)
        {

            var investigationRubricLevel = await _context.InvestigationRubricLevels.Where(x => x.Id == model.Id.Value).FirstOrDefaultAsync();
            if (investigationRubricLevel == null)
                return new BadRequestObjectResult("Sucedio un error");

            var investigationRubricCriterion = await _context.InvestigationRubricCriterions
                .Where(x => x.Id == investigationRubricLevel.InvestigationRubricCriterionId)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationRubricSectionId,
                    x.InvestigationRubricSection.MaxSectionScore,
                    x.InvestigationRubricSection.InvestigationConvocationId,
                })
                .FirstOrDefaultAsync();

            if (investigationRubricCriterion == null)
                return new BadRequestObjectResult("Sucedio un error");

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricCriterion.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            var levels = await _context.InvestigationRubricLevels
                .Where(x => x.InvestigationRubricCriterion.InvestigationRubricSectionId == investigationRubricCriterion.InvestigationRubricSectionId &&
                    x.InvestigationRubricCriterionId != investigationRubricLevel.InvestigationRubricCriterionId)
                .GroupBy(x => new { x.InvestigationRubricCriterionId })
                .Select(x => new
                {
                    RubricCriterionId = x.Key.InvestigationRubricCriterionId,
                    MaxLevel = x.Max(y => y.Score)
                })
                .ToListAsync();

            var totalMaxScore = levels.Select(x => x.MaxLevel).DefaultIfEmpty().Sum();


            if (totalMaxScore + model.Score > investigationRubricCriterion.MaxSectionScore)
            {
                return new BadRequestObjectResult($"El puntaje máximo de toda la seccion no puede permitir mas de {investigationRubricCriterion.MaxSectionScore} puntos en total");
            }

            investigationRubricLevel.Description = model.Description;
            investigationRubricLevel.Score = model.Score;

            await _context.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteLevelAsync(Guid id)
        {
            var investigationRubricLevel = await _context.InvestigationRubricLevels.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigationRubricLevel == null)
                return new BadRequestResult();

            var investigationRubricCriterion = await _context.InvestigationRubricCriterions
                .Where(x => x.Id == investigationRubricLevel.InvestigationRubricCriterionId)
                .Select(x => new
                {
                    x.Id,
                    x.InvestigationRubricSectionId,
                    x.InvestigationRubricSection.MaxSectionScore,
                    x.InvestigationRubricSection.InvestigationConvocationId,
                })
                .FirstOrDefaultAsync();

            if (investigationRubricCriterion == null)
                return new BadRequestResult();

            var hasQualifications = await _context.PostulantRubricQualifications
                .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationRubricCriterion.InvestigationConvocationId);

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");

            _context.InvestigationRubricLevels.Remove(investigationRubricLevel);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        #endregion

        public async Task<IActionResult> OnPostImportAsync(InvestigationRubricImportViewModel model)
        {
            var investigationConvocation = await _context.InvestigationConvocations
                .Where(x => x.Id == model.InvestigationConvocationId)
                .FirstOrDefaultAsync();

            var investigationConvocationToExport = await _context.InvestigationConvocations
                .Where(x => x.Code == model.ConvocationCodeToExport)
                .FirstOrDefaultAsync();

            if (investigationConvocation == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (investigationConvocationToExport == null)
                return new BadRequestObjectResult("No se encontró ninguna convocatoria con este código");

            if (investigationConvocationToExport.Id == investigationConvocation.Id)
                return new BadRequestObjectResult("No puedes exportar la misma convocatoria");

            var hasRubricSection = await _context.InvestigationRubricSections
                .AnyAsync(x => x.InvestigationConvocationId == (investigationConvocation.Id));

            var hasQualifications = await _context.PostulantRubricQualifications
               .AnyAsync(x => x.InvestigationConvocationPostulant.InvestigationConvocationId == investigationConvocation.Id);

            if (hasRubricSection)
                return new BadRequestObjectResult("La convocatoría actual ya presenta una rubrica");

            if (hasQualifications)
                return new BadRequestObjectResult("La rubrica no puede ser editada, ya que presenta calificaciones");


            //Si hemos pasado las validaciones copiaremos la rubrica de la convocatoria que exportaremos
            var rubricSections = await _context.InvestigationRubricSections
                .Where(x=>x.InvestigationConvocationId == investigationConvocationToExport.Id)
                .Select(x => new
                {
                    x.Id,
                    x.Title,
                    x.InvestigationConvocationId,
                    x.MaxSectionScore,
                    criterions = x.InvestigationRubricCriterions
                        .Where(y => y.InvestigationRubricSectionId == x.Id)
                        .Select(y => new
                        {
                            y.Id,
                            y.InvestigationRubricSectionId,
                            y.Name,
                            y.Description,
                            levels = y.InvestigationRubricLevels
                                .Where(z => z.InvestigationRubricCriterionId == y.Id)
                                .Select(z => new
                                {
                                    z.Id,
                                    z.Score,
                                    z.Description,
                                    z.InvestigationRubricCriterionId
                                })
                                .ToList()
                        })
                        .ToList()
                })
                .ToListAsync();

            //Recorremos las secciones, criterios y niveles
            foreach (var sectionItem in rubricSections)
            {
                var investigationRubricSection = new InvestigationRubricSection
                {
                    Title = sectionItem.Title,
                    MaxSectionScore = sectionItem.MaxSectionScore,
                    InvestigationConvocationId = investigationConvocation.Id
                };
                await _context.InvestigationRubricSections.AddAsync(investigationRubricSection);

                foreach (var criterionItem in sectionItem.criterions)
                {
                    var investigationRubricCriterion = new InvestigationRubricCriterion
                    {
                        Name = criterionItem.Name,
                        Description = criterionItem.Description,
                        InvestigationRubricSectionId = investigationRubricSection.Id
                    };
                    await _context.InvestigationRubricCriterions.AddAsync(investigationRubricCriterion);

                    foreach (var levelItem in criterionItem.levels)
                    {
                        var investigationRubricLevel = new InvestigationRubricLevel
                        {
                            Score = levelItem.Score,
                            Description = levelItem.Description,
                            InvestigationRubricCriterionId = investigationRubricCriterion.Id
                        };
                        await _context.InvestigationRubricLevels.AddAsync(investigationRubricLevel);
                    }
                }
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }

    }
}
