using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using AKDEMIC.INFRASTRUCTURE.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.CORE.Services;
using Microsoft.Extensions.Options;
using AKDEMIC.CORE.Options;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using System.Linq.Expressions;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Structs;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Monitor.ViewModels.IncubatorPostulantViewModels;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Monitor.Pages.IncubatorPostulationPage
{
    [Authorize(Roles = GeneralConstants.ROLES.INCUBATORCONVOCATION_MONITOR)]
    public class DetailModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        public DetailModel(
    AkdemicContext context,
    IDataTablesService dataTablesService)
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        public IncubatorPostulantDetailViewModel Input { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid incubatorPostulationId)
        {

            var incubatorPostulation = await _context.IncubatorPostulations
                .Where(x => x.Id == incubatorPostulationId)
                .Select(x => new IncubatorPostulantDetailViewModel
                {
                    Id = x.Id,
                    ConvocationCode = x.IncubatorConvocation.Code,
                    ConvocationName = x.IncubatorConvocation.Name,
                    PostulantFullName = x.User.FullName,
                    PostulantCode = x.User.UserName,
                    Title = x.Title,
                    GeneralGoals = x.GeneralGoals,
                    MonthDuration = x.MonthDuration,
                    Budget = x.Budget,
                    DepartmentText = x.DepartmentText,
                    ProvinceText = x.ProvinceText,
                    DistrictText = x.DistrictText
                })
                .FirstOrDefaultAsync();

            if (incubatorPostulation == null)
                return RedirectToPage("Index");

            Input = incubatorPostulation;

            return Page();
        }

        public async Task<IActionResult> OnGetTeamMembersDatatableAsync(Guid incubatorPostulationId)
        {


            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorPostulationTeamMember, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.UserName;
                    break;
                case "1":
                    orderByPredicate = (x) => x.PaternalSurname;
                    break;
                case "2":
                    orderByPredicate = (x) => x.MaternalSurname;
                    break;
                case "3":
                    orderByPredicate = (x) => x.Name;
                    break;
                case "4":
                    orderByPredicate = (x) => x.CareerText;
                    break;
            }

            var query = _context.IncubatorPostulationTeamMembers
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.UserName,
                    x.PaternalSurname,
                    x.MaternalSurname,
                    x.Name,
                    x.CareerText
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

        public async Task<IActionResult> OnGetAnnexesDatatableAsync(Guid incubatorPostulationId)
        {


            var sentParameters = _dataTablesService.GetSentParameters();

            Expression<Func<IncubatorPostulationAnnex, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.IncubatorConvocationAnnex.Code;
                    break;

                case "1":
                    orderByPredicate = (x) => x.IncubatorConvocationAnnex.Name;
                    break;
                case "2":
                    orderByPredicate = (x) => x.FilePath;
                    break;
            }

            var query = _context.IncubatorPostulationAnnexes
                .Include(x => x.IncubatorConvocationAnnex)
                .Include(x => x.IncubatorPostulation)
                .Include(x => x.IncubatorPostulation.User)
                .Where(x => x.IncubatorPostulationId == incubatorPostulationId)
                .AsNoTracking();

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.IncubatorConvocationAnnex.Code,
                    x.IncubatorConvocationAnnex.Name,
                    x.FilePath
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
