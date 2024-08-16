using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.PublishedBookPage
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

            Expression<Func<PublishedBook, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.MainAuthor);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.PublishingHouse);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.PublishingCity);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.LegalDeposit);
                    break;
            }

            var query = _context.PublishedBooks
                .Where(x => x.UserId == user.Id)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    title = x.Title,
                    mainAuthor = x.MainAuthor,
                    publishingHouse = x.PublishingHouse,
                    publishingCity = x.PublishingCity,
                    legalDeposit = x.LegalDeposit
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

        public async Task<IActionResult> OnPostPublishedBookDeleteAsync(Guid id)
        {
            var publishedBook = await _context.PublishedBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (publishedBook == null)
                return BadRequest("Sucedio un Error");

            _context.PublishedBooks.Remove(publishedBook);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
