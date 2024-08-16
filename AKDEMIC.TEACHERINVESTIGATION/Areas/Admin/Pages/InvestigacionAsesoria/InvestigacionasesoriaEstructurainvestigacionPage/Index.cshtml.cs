using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaEstructurainvestigacionViewModels;
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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.InvestigacionasesoriaEstructurainvestigacionPage
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


        public async Task<IActionResult> OnPostCreateAsync(InvestigacionasesoriaEstructurainvestigacionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionasesoriaestructurainvestigacion = new InvestigacionasesoriaEstructurainvestigacion
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                IdTipotrabajoinvestigacion = viewModel.IdTipotrabajoinvestigacion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionasesoriaEstructurainvestigaciones.AddAsync(investigacionasesoriaestructurainvestigacion);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionasesoriaEstructurainvestigacionEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionasesoriaestructurainvestigacion = await _context.InvestigacionasesoriaEstructurainvestigaciones.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionasesoriaestructurainvestigacion == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionasesoriaestructurainvestigacion.codigo = viewModel.codigo;
            investigacionasesoriaestructurainvestigacion.descripcion = viewModel.descripcion;
            investigacionasesoriaestructurainvestigacion.nombre = viewModel.nombre;
            investigacionasesoriaestructurainvestigacion.IdTipotrabajoinvestigacion = viewModel.IdTipotrabajoinvestigacion;
            investigacionasesoriaestructurainvestigacion.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionasesoriaestructurainvestigacion = await _context.InvestigacionasesoriaEstructurainvestigaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriaestructurainvestigacion == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionasesoriaEstructurainvestigaciones.Remove(investigacionasesoriaestructurainvestigacion);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionasesoriaestructurainvestigacion = await _context.InvestigacionasesoriaEstructurainvestigaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriaestructurainvestigacion == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionasesoriaestructurainvestigacion.Id,
                investigacionasesoriaestructurainvestigacion.codigo,
                investigacionasesoriaestructurainvestigacion.nombre,
                investigacionasesoriaestructurainvestigacion.descripcion,
                investigacionasesoriaestructurainvestigacion.IdTipotrabajoinvestigacion,
                investigacionasesoriaestructurainvestigacion.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionasesoriaEstructurainvestigacion, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }

            var querytipoinvestigacion = _context.InvestigacionasesoriaTipotrabajoinvestigaciones    
                .AsNoTracking();
            var query = _context.InvestigacionasesoriaEstructurainvestigaciones
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x =>x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querytipoinvestigacion,
                    est => est.IdTipotrabajoinvestigacion,
                     tip => tip.Id,
                     (est, tip) => new
                     {
                         nombretipotrabajoinvestigacion = tip.nombre,
                         codigo = est.codigo,
                         IdTipotrabajoinvestigacion = est.IdTipotrabajoinvestigacion,
                         Id = est.Id,
                         nombre = est.nombre,
                         activo = est.activo,
                     })
                .OrderBy(x => x.codigo)
                .ThenBy(x=>x.nombre)
                
                

                
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
        public async Task<IActionResult> OnGetDatatableactividadesAsync(string id = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionasesoriaEstructurainvestigacion, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.descripcion;
                    break;
            }

            var query = _context.InvestigacionasesoriaEstructurainvestigacionesrequisitos
                .AsNoTracking();

           
            int recordsFiltered = await query.CountAsync();

            var data = await query
                .Select(x => new
                {
                    id = x.Id,
                    descripcion = x.descripcion,
                    orden =x.orden,
                    Idestructurainvestigacion = x.Idestructurainvestigacion,

                })
                .Where(x=>x.Idestructurainvestigacion ==Guid.Parse( id))
                .OrderBy(x=>x.orden)
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
