using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublishedBookViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublishedChapterBookViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.PublishedChapterBookPage
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
        public PublishedChapterBookDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGet(Guid publishedChapterBookId)
        {
            var publishedChapterBook = await _context.PublishedChapterBooks
                .Where(x => x.Id == publishedChapterBookId)
                .Select(x => new
                {
                    Id = x.Id,
                    x.User.UserName,
                    x.User.FullName,
                    BookTitle = x.BookTitle,
                    ChapterTitle = x.ChapterTitle,
                    PublishingHouse = x.PublishingHouse,
                    PublishingCity = x.PublishingCity,
                    PublishingYear = x.PublishingYear,
                    StartPage = x.StartPage,
                    EndPage = x.EndPage,
                    DOI = x.DOI,
                    Url = x.Url,
                    MainAuthor = x.MainAuthor,
                    ISBN = x.ISBN,
                }).FirstOrDefaultAsync();

            if (publishedChapterBook == null)
                return RedirectToPage("/Index");

            Input = new PublishedChapterBookDetailViewModel
            {
                Id = publishedChapterBook.Id,
                BookTitle = publishedChapterBook.BookTitle,
                ChapterTitle = publishedChapterBook.ChapterTitle,
                UserName = publishedChapterBook.UserName,
                UserFullName = publishedChapterBook.FullName,
                PublishingHouse = publishedChapterBook.PublishingHouse,
                PublishingCity = publishedChapterBook.PublishingCity,
                PublishingYear = publishedChapterBook.PublishingYear,
                StartPage = publishedChapterBook.StartPage,
                EndPage = publishedChapterBook.EndPage,
                Url = publishedChapterBook.Url,
                DOI = publishedChapterBook.DOI,
                MainAuthor = publishedChapterBook.MainAuthor,
                ISBN = publishedChapterBook.ISBN
            };

            return Page();
        }
        //Datatable de Autor
        public async Task<IActionResult> OnGetAuthorDatatableAsync(Guid publishedChapterBookId)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedChapterBookAuthor, dynamic>> orderByPredicate = null;
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

            var query = _context.PublishedChapterBookAuthors
                .Where(x => x.PublishedChapterBookId == publishedChapterBookId && x.PublishedChapterBook.UserId == user.Id)
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
        public async Task<IActionResult> OnGetFileDatatableAsync(Guid publishedChapterBookId)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedChapterBookFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "1":
                    orderByPredicate = (x) => x.FilePath;
                    break;

            }

            var query = _context.PublishedChapterBookFiles
                .Where(x => x.PublishedChapterBookId == publishedChapterBookId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.FilePath,
                    x.Name,
                    x.PublishedChapterBookId
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