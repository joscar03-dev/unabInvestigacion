using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ConferenceViewModels;
using System.Linq;
using AKDEMIC.CORE.Extensions;
using Microsoft.EntityFrameworkCore;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.ConferencePage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," + GeneralConstants.ROLES.PUBLICATION_UNIT)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager
        )
        {
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
        }

        [BindProperty]
        public ConferenceDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(Guid conferenceId)
        {
            var conference = await _context.Conferences
                .Where(x => x.Id == conferenceId)
                .Select(x => new
                {
                    Id = x.Id,
                    x.User.FullName,
                    x.User.UserName,
                    OpusTypeName = x.OpusType.Name ?? "",
                    TypeName = TeacherInvestigationConstants.Conference.Type.VALUES.ContainsKey(x.Type) ?
                        TeacherInvestigationConstants.Conference.Type.VALUES[x.Type] : "",
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

            Input = new ConferenceDetailViewModel
            {
                Id = conference.Id,
                UserName = conference.UserName,
                UserFullName = conference.FullName,
                OpusTypeName = conference.OpusTypeName,
                TypeName = conference.TypeName,
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
        //Datatable de Autor
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
        //Datatable de Archivo
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


    }
}
