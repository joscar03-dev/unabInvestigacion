using AKDEMIC.CORE.Constants;
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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublishedBookViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.PublishedBookPage
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
        public PublishedBookDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(Guid publishedBookId)
        {
            var publishedBook = await _context.PublishedBooks
                .Where(x => x.Id == publishedBookId)
                .Select(x => new
                {
                    Id = x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    Title = x.Title,
                    PublishingHouse = x.PublishingHouse,
                    PublishingCity = x.PublishingCity,
                    PublishingYear = x.PublishingYear,
                    PagesCount = x.PagesCount,
                    LegalDeposit = x.LegalDeposit,
                    Url = x.Url,
                    MainAuthor = x.MainAuthor,
                    ISBN = x.ISBN,
                }).FirstOrDefaultAsync();

            if (publishedBook == null)
                return RedirectToPage("/Index");

            Input = new PublishedBookDetailViewModel
            {
                Id = publishedBook.Id,
                Title = publishedBook.Title,
                UserName = publishedBook.UserName,
                UserFullName = publishedBook.FullName,
                PublishingHouse = publishedBook.PublishingHouse,
                PublishingCity = publishedBook.PublishingCity,
                PublishingYear = publishedBook.PublishingYear,
                PagesCount = publishedBook.PagesCount,
                LegalDeposit = publishedBook.LegalDeposit,
                Url = publishedBook.Url,
                MainAuthor = publishedBook.MainAuthor,
                ISBN = publishedBook.ISBN
            };

            return Page();
        }
        //Datatable de Autor
        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid publishedBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedBookAuthor, dynamic>> orderByPredicate = null;
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

            var query = _context.PublishedBookAuthors
                .Where(x => x.PublishedBookId == publishedBookId && x.PublishedBook.UserId == user.Id)
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
        public async Task<IActionResult> OnGetFileDatatableAsync(Guid publishedBookId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedBookFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.PublishedBookFiles
                .Where(x => x.PublishedBookId == publishedBookId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.PublishedBookId
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