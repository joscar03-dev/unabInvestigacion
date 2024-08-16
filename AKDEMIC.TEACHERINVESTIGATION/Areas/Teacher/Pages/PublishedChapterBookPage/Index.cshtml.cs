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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublishedChapterBookPage
{
    [Authorize(Roles = GeneralConstants.ROLES.RESEARCHERS)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager
        )
        {

            _dataTablesService = dataTablesService;
            _context = context;
            _userManager = userManager;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedChapterBook, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.BookTitle);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.ChapterTitle);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.PublishingHouse);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.PublishingCity);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.DOI);
                    break;
            }

            var query = _context.PublishedChapterBooks
                .Where(x => x.UserId == user.Id)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.BookTitle.ToUpper().Contains(searchValue.ToUpper()) ||
                                        x.ChapterTitle.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    bookTitle = x.BookTitle,
                    chapterTitle = x.ChapterTitle,
                    publishingHouse = x.PublishingHouse,
                    publishingCity = x.PublishingCity,
                    doi = x.DOI
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

        public async Task<IActionResult> OnPostPublishedChapterBookDeleteAsync(Guid id)
        {
            var publishedChapterBook = await _context.PublishedChapterBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (publishedChapterBook == null)
                return BadRequest("Sucedio un Error");

            _context.PublishedChapterBooks.Remove(publishedChapterBook);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
