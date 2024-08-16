using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Student.ViewModels.IncubatorConvocationViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Student.Pages.IncubatorConvocationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.STUDENTS)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public DetailModel(
            AkdemicContext context,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials)
        {
            _context = context;
            _userManager = userManager;
            _storageCredentials = storageCredentials;
        }

        public IncubatorConvocationDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorConvocationId)
        {
            var incubatorConvocation = await _context.IncubatorConvocations
                .Where(x => x.Id == incubatorConvocationId)
                .Select(x => new IncubatorConvocationDetailViewModel
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    InscriptionEndDate = x.InscriptionEndDate.ToLocalDateTimeFormat(),
                    InscriptionStartDate = x.InscriptionStartDate.ToLocalDateTimeFormat(),
                    Requirements = x.Requirements,
                    PicturePath = x.PicturePath,
                    DocumentPath = x.DocumentPath
                })
                .FirstOrDefaultAsync();

            if (incubatorConvocation == null)
                return RedirectToPage("Index");

            Input = incubatorConvocation;
            return Page();
        }

        public async Task<IActionResult> OnGetIncubatorConvocationAnnexAsync(Guid incubatorConvocationId) 
        {
            var data =  await _context.IncubatorConvocationAnnexes
                .Where(x => x.IncubatorConvocationId == incubatorConvocationId)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name
                })
                .ToListAsync();

            return new OkObjectResult(data);
        }

        public async Task<IActionResult> OnPostAsync(IncubatorPostulationViewModel viewModel)
        {
            var today = DateTime.UtcNow;
            var incubatorConvocation = await _context.IncubatorConvocations
                .Where(x => x.Id == viewModel.IncubatorConvocationId)
                .Select(x => new 
                {
                    x.Id,
                    x.InscriptionEndDate,
                    x.InscriptionStartDate,
                    IncubatorConvocationAnnexes = x.IncubatorConvocationAnnexes
                        .Where(y => y.IncubatorConvocationId == x.Id)
                        .Select(y => new 
                        {
                            y.Id,
                            y.Code,
                            y.Name,
                            y.IncubatorConvocationId
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);

            if (incubatorConvocation == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (user == null)
                return new BadRequestObjectResult("Sucedio un error");

            //Validaciones para el asesor y co asesor ?

            //Validaciones que el usuario ya no haya postulado a esta convocatoria de incubacion
            if (await _context.IncubatorPostulations.AnyAsync(x => x.IncubatorConvocationId == incubatorConvocation.Id && x.UserId == user.Id))
                return BadRequest("Ya existe una postulación para esta convocatoria");

            if (!(incubatorConvocation.InscriptionStartDate <= today && incubatorConvocation.InscriptionEndDate >= today))
                return BadRequest("Debe estar dentro del rango de fechas de inscripción");

            var incubatorPostulation = new IncubatorPostulation
            {
                Title = viewModel.Title,
                Budget = viewModel.Budget,
                GeneralGoals = viewModel.GeneralGoals,
                MonthDuration = viewModel.MonthDuration,
                IncubatorConvocationId = incubatorConvocation.Id,
                UserId = user.Id,
                DepartmentId = viewModel.DepartmentId,
                DepartmentText = viewModel.DepartmentText,
                ProvinceId = viewModel.ProvinceId,
                ProvinceText = viewModel.ProvinceText,
                DistrictId = viewModel.DistrictId,
                DistrictText = viewModel.DistrictText,
                RegisterDate = DateTime.UtcNow,
                ReviewState = TeacherInvestigationConstants.IncubatorPostulation.ReviewState.PENDING,
            };

            await _context.IncubatorPostulations.AddAsync(incubatorPostulation);

            foreach (var item in viewModel.IncubatorPostulationAnnexes)
            {
                var incubatorConvocationAnnexExist = incubatorConvocation.IncubatorConvocationAnnexes
                    .Any(x => x.Id == item.IncubatorConvocationAnnexId);

                if (!incubatorConvocationAnnexExist)
                    return BadRequest("Sucedio un error");

                var storage = new CloudStorageService(_storageCredentials);

                string fileUrl = await storage.UploadFile(item.AnnexFile.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INCUBATORPOSTULATION_ANNEXES,
                Path.GetExtension(item.AnnexFile.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                var incubatorPostulationAnnex = new IncubatorPostulationAnnex
                {
                    IncubatorPostulationId = incubatorPostulation.Id,
                    IncubatorConvocationAnnexId = item.IncubatorConvocationAnnexId,
                    FilePath = fileUrl
                };
                await _context.IncubatorPostulationAnnexes.AddAsync(incubatorPostulationAnnex);
            }


            await _context.SaveChangesAsync();

            return new OkResult();
        }        
    }
}
