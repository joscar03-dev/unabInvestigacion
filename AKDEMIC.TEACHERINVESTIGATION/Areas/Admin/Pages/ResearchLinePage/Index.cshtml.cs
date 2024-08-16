using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.ResearchLineViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.ResearchLinePage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;

        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService
        )
        {
            _context = context;
            _dataTablesService = dataTablesService;
        }

        [BindProperty(SupportsGet = true)]
        public Guid ResearchLineCategoryId { get; set; }
        public string ResearchLineCategoryName { get; set; }

        
        public async Task<IActionResult> OnGet(Guid researchLineCategoryId)
        {
            var researchLineCategory = await _context.ResearchLineCategories
                .Where(c => c.Id == researchLineCategoryId)
                .FirstOrDefaultAsync();
            ResearchLineCategoryName = researchLineCategory.Name;
            return Page();
        }


        public async Task<IActionResult> OnPostCreateAsync(ResearchLineCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var researchLine = new ResearchLine
            {
                Code = viewModel.Code,
                Name = viewModel.Name,
                ResearchLineCategoryId = viewModel.ResearchLineCategoryId,
            };

            await _context.ResearchLines.AddAsync(researchLine);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(ResearchLineEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var researchLine = await _context.ResearchLines.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (researchLine == null) return new BadRequestObjectResult("Sucedio un error");

            researchLine.Code = viewModel.Code;
            researchLine.ResearchLineCategoryId = viewModel.ResearchLineCategoryId;
            researchLine.Name = viewModel.Name;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var researchLine = await _context.ResearchLines.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (researchLine == null) return new BadRequestObjectResult("Sucedio un error");

            _context.ResearchLines.Remove(researchLine);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var researchLine = await _context.ResearchLines.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (researchLine == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                researchLine.Id,
                researchLine.Code,
                researchLine.Name,
                researchLine.ResearchLineCategoryId
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<ResearchLine, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.ResearchLines
                .Where(x=> x.ResearchLineCategoryId == ResearchLineCategoryId)
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.Code.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.Name.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    x.ResearchLineCategoryId,
                    ResearchLineCategoryName= x.ResearchLineCategory.Name,
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
