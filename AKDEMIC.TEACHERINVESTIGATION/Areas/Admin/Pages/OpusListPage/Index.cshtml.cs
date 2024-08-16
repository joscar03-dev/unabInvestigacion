using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.OpusListViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.OpusListdetailViewModels;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.OpusListPage
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


        public async Task<IActionResult> OnPostCreateAsync(OpusListCreateViewModel viewModel)
        {
          /*  if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var opusType = new OpusList
            {
                Code = viewModel.Code,
                Name = viewModel.Name,
            };

            await _context.OpusLists.AddAsync(opusType);
            await _context.SaveChangesAsync();
          */
            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(OpusListEditViewModel viewModel)
        {/*
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var opusType = await _context.OpusLists.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (opusType == null) return new BadRequestObjectResult("Sucedio un error");

            opusType.Code = viewModel.Code;
            opusType.Name = viewModel.Name;
            await _context.SaveChangesAsync();*/

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var opusType = await _context.OpusLists.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (opusType == null) return new BadRequestObjectResult("Sucedio un error");

            _context.OpusLists.Remove(opusType);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var opusType = await _context.OpusLists.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (opusType == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                opusType.Id,
                opusType.Code,
                opusType.Name,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<OpusList, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Code;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Name;
                    break;
            }

            var query = _context.OpusLists
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

        public async Task<IActionResult> OnGetDatatableValorAsync(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<OpusListdetail, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Id;
                    break;
            }


            var queryvalor = _context.OpusLists.AsNoTracking();

            var query = _context.OpusListsdetails
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryvalor,
                  listvalor1 => listvalor1.IdList,
                   list1 => list1.Id,
                    (listvalor1, list1) => new { listvalor1, list1 })

                .Select(x => new
                {
                    nombrevalor = x.listvalor1.Name,
                    id = x.listvalor1.Id,
                    IdType = x.listvalor1.IdList,

                })
                .Where(x => x.IdType == id);

            int recordsFiltered = await query.CountAsync();


            var data = await query.Skip(sentParameters.PagingFirstRecord)
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

        public async Task<IActionResult> OnPostCreateValorAsync(OpusListdetailCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");



            var opustypevalor = new OpusListdetail
            {
                Name = viewModel.Name,        
                IdList = viewModel.IdList,
            };

            await _context.OpusListsdetails.AddAsync(opustypevalor);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostDeletevalorAsync(Guid id)
        {
            var opustypevalor = await _context.OpusListsdetails.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (opustypevalor == null) return new BadRequestObjectResult("Sucedio un error");

            _context.OpusListsdetails.Remove(opustypevalor);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

    }
}
