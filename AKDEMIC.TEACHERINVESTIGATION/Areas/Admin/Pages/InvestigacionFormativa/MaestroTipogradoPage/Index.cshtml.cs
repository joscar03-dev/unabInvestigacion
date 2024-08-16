using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroTipogradoViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.MaestroTipogradoPage
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
        public void OnGet()
        {
        }


        public async Task<IActionResult> OnPostCreateAsync(MaestroTipogradoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var tipogrado = new MaestroTipogrado
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroTipogrados.AddAsync(tipogrado);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroTipogradoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrotipogrado = await _context.MaestroTipogrados.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestrotipogrado == null) return new BadRequestObjectResult("Sucedio un error");

            maestrotipogrado.codigo = viewModel.codigo;
            maestrotipogrado.descripcion = viewModel.descripcion;
            maestrotipogrado.nombre = viewModel.nombre;
            maestrotipogrado.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestrotipogrado = await _context.MaestroTipogrados.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrotipogrado == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroTipogrados.Remove(maestrotipogrado);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestrotipogrado = await _context.MaestroTipogrados.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrotipogrado == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestrotipogrado.Id,
                maestrotipogrado.codigo,
                maestrotipogrado.nombre,
                maestrotipogrado.descripcion,
                maestrotipogrado.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroTipogrado, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.MaestroTipogrados
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    x.codigo,
                    x.nombre,
                    x.descripcion,
                    x.activo,
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
