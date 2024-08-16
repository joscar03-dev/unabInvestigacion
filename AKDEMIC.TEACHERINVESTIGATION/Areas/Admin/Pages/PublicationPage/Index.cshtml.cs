using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.PublicationPage
{

    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," + GeneralConstants.ROLES.PUBLICATION_UNIT)]
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

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, int workCategory = 0, Guid? opusTypeId = null)
        {
        

            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<Publication, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.WorkCategory);
                    break;
                case "1":
                    orderByPredicate = ((x) => x.OpusType.Name);
                    break;
                case "2":
                    orderByPredicate = ((x) => x.Title);
                    break;
                case "3":
                    orderByPredicate = ((x) => x.User.UserName);
                    break;
                case "4":
                    orderByPredicate = ((x) => x.User.FullName);
                    break;
                case "5":
                    orderByPredicate = ((x) => x.PublishDate);
                    break;
            }

            var query = _context.Publications
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => x.Title.ToUpper().Contains(searchValue.ToUpper()));
            }

            if (workCategory != 0)
            {
                query = query.Where(x => x.WorkCategory == workCategory);
            }

            if (opusTypeId.HasValue && opusTypeId != null)
            {
                query = query.Where(x => x.OpusTypeId == opusTypeId);
            }

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    id = x.Id,
                    workCategory = TeacherInvestigationConstants.Publication.WorkCategory.VALUES.ContainsKey(x.WorkCategory) ?
                        TeacherInvestigationConstants.Publication.WorkCategory.VALUES[x.WorkCategory] : "",
                    opusType = x.OpusType.Name,
                    x.User.UserName,
                    x.User.FullName,
                    title = x.Title,
                    publishDate = x.PublishDate.ToLocalDateFormat()
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
