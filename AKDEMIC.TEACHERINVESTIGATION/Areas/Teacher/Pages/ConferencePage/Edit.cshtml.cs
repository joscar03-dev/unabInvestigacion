using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Options;
using AKDEMIC.CORE.Services;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.ConferenceViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using System;
using AKDEMIC.CORE.Extensions;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using System.Linq.Expressions;
using System.IO;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.ConferencePage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class EditModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public EditModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _storageCredentials = storageCredentials;
            _context = context;
            _dataTablesService = dataTablesService;
            _userManager = userManager;
        }

        [BindProperty]
        public ConferenceEditViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid conferenceId)
        {
            var user = await _userManager.GetUserAsync(User);

            var conference = await _context.Conferences
                .Where(x => x.Id == conferenceId && x.UserId == user.Id)
                .Select(x => new
                {
                    Id = x.Id,
                    OpusTypeId = x.OpusTypeId,
                    Type = x.Type,
                    Title = x.Title,
                    Name = x.Name,
                    OrganizerInstitution = x.OrganizerInstitution,
                    Country = x.Country,
                    City = x.City,
                    StartDate = x.StartDate.ToLocalDateFormat(),
                    EndDate = x.EndDate.ToLocalDateFormat(),
                    MainAuthor = x.MainAuthor,
                    ISBN = x.ISBN,
                    ISSN = x.ISSN,
                    DOI = x.DOI,
                    UrlEvent = x.UrlEvent

                }).FirstOrDefaultAsync();

            if (conference == null)
                return RedirectToPage("/Index");

            Input = new ConferenceEditViewModel
            {
                Id = conference.Id,
                OpusTypeId = conference.OpusTypeId,
                Type = conference.Type,
                Title = conference.Title,
                Name = conference.Name,
                OrganizerInstitution = conference.OrganizerInstitution,
                Country = conference.Country,
                City = conference.City,
                StartDate = conference.StartDate,
                EndDate = conference.EndDate,
                MainAuthor = conference.MainAuthor,
                ISBN = conference.ISBN,
                ISSN = conference.ISSN,
                DOI = conference.DOI,
                UrlEvent = conference.UrlEvent
            };

            return Page();
        }

        #region Author

        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid conferenceId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<ConferenceAuthor, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "1":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Email;
                    break;
                case "4":
                    orderByPredicate = (x) => x.Dni;
                    break;
            }

            var query = _context.ConferenceAuthors
                .Where(x => x.ConferenceId == conferenceId && x.Conference.UserId == user.Id)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.ToLocalDateFormat() : "",
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.Email,
                    x.Dni
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostCreateAuthorAsync(ConferenceAuthorCreateViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var conference = await _context.Conferences.Where(x => x.Id == viewModel.ConferenceId && x.UserId == user.Id).FirstOrDefaultAsync();

            if (conference == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (await _context.ConferenceAuthors.Where(x => x.ConferenceId == conference.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            var conferenceAuthor = new ConferenceAuthor
            {
                ConferenceId = conference.Id,
                PaternalSurname = viewModel.PaternalSurname,
                MaternalSurname = viewModel.MaternalSurname,
                Name = viewModel.Name,
                Email = viewModel.Email,
                Dni = viewModel.Dni
            };

            await _context.ConferenceAuthors.AddAsync(conferenceAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDetailAuthorAsync(Guid id)
        {
            var conferenceAuthor = await _context.ConferenceAuthors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (conferenceAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                conferenceAuthor.PaternalSurname,
                conferenceAuthor.MaternalSurname,
                conferenceAuthor.Name,
                conferenceAuthor.Email,
                conferenceAuthor.Dni,
                conferenceAuthor.Id,
                conferenceAuthor.ConferenceId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEditAuthorAsync(ConferenceAuthorEditViewModel viewModel)
        {
            var user = await _userManager.GetUserAsync(User);

            var conferenceAuthor = await _context.ConferenceAuthors
                .Where(x => x.Id == viewModel.Id && x.ConferenceId == viewModel.ConferenceId && x.Conference.UserId == user.Id)
                .FirstOrDefaultAsync();

            if (conferenceAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (await _context.ConferenceAuthors.Where(x => x.ConferenceId == viewModel.ConferenceId && x.Id != viewModel.Id).AnyAsync(x => x.Dni == viewModel.Dni))
                return new BadRequestObjectResult("Ya existe un autor con este dni");

            conferenceAuthor.PaternalSurname = viewModel.PaternalSurname;
            conferenceAuthor.MaternalSurname = viewModel.MaternalSurname;
            conferenceAuthor.Name = viewModel.Name;
            conferenceAuthor.Email = viewModel.Email;
            conferenceAuthor.Dni = viewModel.Dni;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAuthorAsync(Guid id)
        {
            var conferenceAuthor = await _context.ConferenceAuthors
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (conferenceAuthor == null)
                return new BadRequestObjectResult("Sucedio un error");

            _context.ConferenceAuthors.Remove(conferenceAuthor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }


        #endregion

        #region File

        public async Task<IActionResult> OnGetFileDatatableAsync(Guid conferenceId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<ConferenceFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.ConferenceFiles
                .Where(x => x.ConferenceId == conferenceId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.ConferenceId
                })
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;

            var result = new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostCreateFile(ConferenceFileCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var conference = await _context.Conferences.Where(x => x.Id == viewModel.ConferenceId).FirstOrDefaultAsync();

            if (conference == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (viewModel.File == null)
                return new BadRequestObjectResult("Debe seleccionar un archivo");

            var storage = new CloudStorageService(_storageCredentials);

            string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.CONFERENCE_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

            var conferenceFile = new ConferenceFile
            {
                ConferenceId = conference.Id,
                Name = viewModel.Name,
                FilePath = fileUrl
            };

            await _context.ConferenceFiles.AddAsync(conferenceFile);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditFileAsync(ConferenceFileEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var conferenceFile = await _context.ConferenceFiles.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (conferenceFile == null) return new BadRequestObjectResult("Sucedio un error");

            conferenceFile.Name = viewModel.Name;

            var storage = new CloudStorageService(_storageCredentials);

            if (viewModel.File != null)
            {
                string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.CONFERENCE_DOCUMENTS,
                Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);

                conferenceFile.FilePath = fileUrl;
            }

            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailFileAsync(Guid id)
        {
            var conferenceFile = await _context.ConferenceFiles
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (conferenceFile == null)
                return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                conferenceFile.Id,
                conferenceFile.Name,
                conferenceFile.FilePath,
                conferenceFile.ConferenceId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostDeleteFileAsync(Guid id)
        {
            var conferenceFile = await _context.ConferenceFiles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (conferenceFile == null) return new BadRequestObjectResult("Sucedio un error");

            _context.ConferenceFiles.Remove(conferenceFile);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

        #endregion



        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise el formulario");

            var conference = await _context.Conferences.Where(x => x.Id == Input.Id).FirstOrDefaultAsync();

            if (conference == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (string.IsNullOrEmpty(Input.StartDate) || string.IsNullOrEmpty(Input.EndDate))
                return new BadRequestObjectResult("Debe especificar la fecha de inicio y fecha de fin");


            DateTime startDate = ConvertHelpers.DatepickerToUtcDateTime(Input.StartDate);
            DateTime endDate = ConvertHelpers.DatepickerToUtcDateTime(Input.EndDate);

            if (endDate < startDate)
                return new BadRequestObjectResult("La fecha de fin no puede ser menor a la fecha de inicio");

            conference.OpusTypeId = Input.OpusTypeId;
            conference.Type = Input.Type;
            conference.Title = Input.Title;
            conference.Name = Input.Name;
            conference.OrganizerInstitution = Input.OrganizerInstitution;
            conference.Country = Input.Country;
            conference.City = Input.City;
            conference.StartDate = startDate;
            conference.EndDate = endDate;
            conference.MainAuthor = Input.MainAuthor;
            conference.DOI = Input.DOI;
            conference.ISBN = Input.ISBN;
            conference.ISSN = Input.ISSN;
            conference.UrlEvent = Input.UrlEvent;

            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
