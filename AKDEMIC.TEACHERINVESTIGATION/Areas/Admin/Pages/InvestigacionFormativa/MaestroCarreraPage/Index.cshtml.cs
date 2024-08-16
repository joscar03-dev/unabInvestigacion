using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroCarreraViewModels;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.MaestroCarreraPage
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


        public async Task<IActionResult> OnPostCreateAsync(MaestroCarreraCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrocarrera = new MaestroCarrera
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                IdFacultad = viewModel.IdFacultad,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroCarreras.AddAsync(maestrocarrera);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroCarreraEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrocarrera = await _context.MaestroCarreras.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestrocarrera == null) return new BadRequestObjectResult("Sucedio un error");

            maestrocarrera.codigo = viewModel.codigo;
            maestrocarrera.descripcion = viewModel.descripcion;
            maestrocarrera.nombre = viewModel.nombre;
            maestrocarrera.IdFacultad = viewModel.IdFacultad;
            maestrocarrera.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestrocarrera = await _context.MaestroCarreras.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrocarrera == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroCarreras.Remove(maestrocarrera);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestrocarrera = await _context.MaestroCarreras.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrocarrera == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestrocarrera.Id,
                maestrocarrera.codigo,
                maestrocarrera.nombre,
                maestrocarrera.descripcion,
                maestrocarrera.IdFacultad,
                maestrocarrera.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroCarrera, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }

            var queryfacultades = _context.MaestroFacultades    
                .AsNoTracking();
            var query = _context.MaestroCarreras
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryfacultades, 
                    car => car.IdFacultad,                    
                     fac => fac.Id,
                     (car, fac) => new
                     {
                         nombrefacultad = fac.nombre,
                         IdFacultad = car.IdFacultad,
                         Id=car.Id,
                         codigo=car.codigo,
                         nombre=car.nombre,
                         descripcion = car.descripcion,
                         activo = car.activo,
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
