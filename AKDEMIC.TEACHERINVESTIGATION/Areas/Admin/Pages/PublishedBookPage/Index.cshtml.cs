using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.PublishedBookPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," + GeneralConstants.ROLES.PUBLICATION_UNIT)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService
        )
        {
            _dataTablesService = dataTablesService;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<PublishedBook, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Title);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.PublishingHouse);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.PublishingCity);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.LegalDeposit);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
            }

            var query = _context.PublishedBooks
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
                    x.User.UserName,
                    x.User.FullName,
                    title = x.Title,
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
    }
}

