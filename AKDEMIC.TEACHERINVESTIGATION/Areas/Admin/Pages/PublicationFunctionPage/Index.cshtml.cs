using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublicationFunctionViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.PublicationFunctionPage
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


        public async Task<IActionResult> OnPostCreateAsync(PublicationFunctionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publicationFunction = new PublicationFunction
            {
                Code = viewModel.Code,
                Name = viewModel.Name
            };

            await _context.PublicationFunctions.AddAsync(publicationFunction);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(PublicationFunctionEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var publicationFunction = await _context.PublicationFunctions.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (publicationFunction == null) return new BadRequestObjectResult("Sucedio un error");

            publicationFunction.Code = viewModel.Code;
            publicationFunction.Name = viewModel.Name;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var publicationFunction = await _context.PublicationFunctions.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (publicationFunction == null) return new BadRequestObjectResult("Sucedio un error");

            _context.PublicationFunctions.Remove(publicationFunction);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var publicationFunction = await _context.PublicationFunctions.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (publicationFunction == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                publicationFunction.Id,
                publicationFunction.Code,
                publicationFunction.Name
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<PublicationFunction, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.PublicationFunctions
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
