using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.IndexPlaceViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.IndexPlacePage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.PUBLICATION_UNIT + "," +
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
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostCreateAsync(IndexPlaceCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var indexPlace = new IndexPlace
            {
                Code = viewModel.Code,
                Name = viewModel.Name
            };

            await _context.IndexPlaces.AddAsync(indexPlace);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(IndexPlaceEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var indexPlace = await _context.IndexPlaces.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (indexPlace == null) return new BadRequestObjectResult("Sucedio un error");

            indexPlace.Code = viewModel.Code;
            indexPlace.Name = viewModel.Name;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var indexPlace = await _context.IndexPlaces.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (indexPlace == null) return new BadRequestObjectResult("Sucedio un error");

            _context.IndexPlaces.Remove(indexPlace);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var indexPlace = await _context.IndexPlaces.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (indexPlace == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                indexPlace.Id,
                indexPlace.Code,
                indexPlace.Name
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<IndexPlace, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.IndexPlaces
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
                    x.Name
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
