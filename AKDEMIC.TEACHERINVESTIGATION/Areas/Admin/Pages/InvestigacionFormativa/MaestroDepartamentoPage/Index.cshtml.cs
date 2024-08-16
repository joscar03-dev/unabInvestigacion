using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroDepartamentoViewModels;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.MaestroDepartamentoPage
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


        public async Task<IActionResult> OnPostCreateAsync(MaestroDepartamentoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrofacultad = new MaestroDepartamento
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                IdFacultad=viewModel.IdFacultad,
                activo = viewModel.activo ?? "0",
            };
            await _context.MaestroDepartamentos.AddAsync(maestrofacultad);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroDepartamentoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrofacultad = await _context.MaestroDepartamentos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestrofacultad == null) return new BadRequestObjectResult("Sucedio un error");

            maestrofacultad.codigo = viewModel.codigo;
            maestrofacultad.descripcion = viewModel.descripcion;
            maestrofacultad.nombre = viewModel.nombre;
            maestrofacultad.IdFacultad = viewModel.IdFacultad;
            maestrofacultad.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestrofacultad = await _context.MaestroDepartamentos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrofacultad == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroDepartamentos.Remove(maestrofacultad);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestrofacultad = await _context.MaestroDepartamentos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrofacultad == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestrofacultad.Id,
                maestrofacultad.codigo,
                maestrofacultad.nombre,
                maestrofacultad.descripcion,
                maestrofacultad.IdFacultad,
                maestrofacultad.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroDepartamento, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.MaestroDepartamentos
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();
            var queryfacultades = _context.MaestroFacultades.AsNoTracking();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryfacultades,
                    departamentos1 => departamentos1.IdFacultad,
                    facultad1 => facultad1.Id,
                     (departamentos1, facultad1) => new { departamentos1, facultad1 })
                .Select(x => new
                {
                   Id= x.departamentos1.Id,
                    codigo= x.departamentos1.codigo,
                    nombre= x.departamentos1.nombre,
                    descripcion=x.departamentos1.descripcion,
                    activo= x.departamentos1.activo,
                    nombrefacultad = x.facultad1.nombre,
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
