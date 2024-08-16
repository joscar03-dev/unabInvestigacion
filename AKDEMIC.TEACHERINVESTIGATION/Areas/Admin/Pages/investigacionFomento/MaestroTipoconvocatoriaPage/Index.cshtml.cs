using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;

using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.MaestroTipoconvocatoriaViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFomento.MaestroTipoconvocatoriaPage
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
        Guid xIdOficina = Guid.Parse("e0557aa7-6404-11ee-b7b1-16d13ee00159");

        public async Task<IActionResult> OnPostCreateAsync(MaestroTipoconvocatoriaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroarea = new MaestroTipoconvocatoria
            {
                IdOficina=xIdOficina,
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroTipoconvocatorias.AddAsync(maestroarea);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroTipoconvocatoriaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroarea = await _context.MaestroTipoconvocatorias.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestroarea == null) return new BadRequestObjectResult("Sucedio un error");

            maestroarea.codigo = viewModel.codigo;
            maestroarea.descripcion = viewModel.descripcion;
            maestroarea.nombre = viewModel.nombre;
            maestroarea.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestroarea = await _context.MaestroTipoconvocatorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroarea == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroTipoconvocatorias.Remove(maestroarea);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestroarea = await _context.MaestroTipoconvocatorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroarea == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestroarea.Id,
                maestroarea.codigo,
                maestroarea.nombre,
                maestroarea.descripcion,
                maestroarea.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroTipoconvocatoria, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.MaestroTipoconvocatorias
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => (x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                                   x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim())) && x.IdOficina == xIdOficina);

            }
            else
            {
                query = query.Where(x => x.IdOficina == xIdOficina);
            };
               
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
