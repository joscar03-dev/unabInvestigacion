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
using System.Threading.Tasks;
using System;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Constants.Systems;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.Pages.ConferencePage
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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, int type = 0, Guid? opusTypeId = null)
        {
            var user = await _userManager.GetUserAsync(User);

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<Conference, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.OpusType.Name);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.Type);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Title);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.StartDate);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.EndDate);
                    break;
            }

            var query = _context.Conferences
                .Where(x => x.UserId == user.Id)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (type != 0)
            {
                query = query.Where(x => x.Type == type);
            }

            if (opusTypeId != null)
            {
                query = query.Where(x => x.OpusTypeId == opusTypeId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    type = TeacherInvestigationConstants.Conference.Type.VALUES.ContainsKey(x.Type) ?
                        TeacherInvestigationConstants.Conference.Type.VALUES[x.Type] : "",
                    opusType = x.OpusType.Name,
                    title = x.Title,
                    startDate = x.StartDate.ToLocalDateFormat(),
                    endDate = x.EndDate.ToLocalDateFormat()
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

        public async Task<IActionResult> OnPostConferenceDeleteAsync(Guid id)
        {
            var conference = await _context.Conferences.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (conference == null)
                return BadRequest("Sucedio un Error");

            _context.Conferences.Remove(conference);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
    }
}
