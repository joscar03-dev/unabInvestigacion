using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.Pages.EventPage
{
    [Authorize(Roles = GeneralConstants.ROLES.BUSINESS_INCUBATOR_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT + "," +
        GeneralConstants.ROLES.PUBLICATION_UNIT + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        protected readonly AkdemicContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
    IDataTablesService dataTablesService,
    AkdemicContext context,
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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<Event, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {

                case "1":
                    orderByPredicate = ((x) => x.Title);
                    break;

            }

            var query = _context.Events
                .Where(x => x.Unit.UserId == user.Id)
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
                    x.Id,
                    x.Title,
                    x.Description,
                    EventDate = x.EventDate.ToLocalDateFormat(),
                    x.VideoUrl,
                    x.Cost,
                    x.Organizer,
                    x.Unit.Name,
                    x.PicturePath,
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

        public async Task<IActionResult> OnGetDeleteAsync(Guid id)
        {
            var e = await _context.Events.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (e == null)
                return BadRequest("Sucedio un Error");

            _context.Events.Remove(e);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
