using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroLineaViewModels;
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
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.MaestroLineaPage
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


        public async Task<IActionResult> OnPostCreateAsync(MaestroLineaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrolinea = new MaestroLinea
            {
                codigo = viewModel.codigo,
                titulo = viewModel.titulo,
                descripcion = viewModel.descripcion,
                IdAreaacademica= viewModel.IdAreaacademica,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroLineas.AddAsync(maestrolinea);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroLineaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrolinea = await _context.MaestroLineas.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestrolinea == null) return new BadRequestObjectResult("Sucedio un error");

            maestrolinea.codigo = viewModel.codigo;
            maestrolinea.descripcion = viewModel.descripcion;
            maestrolinea.titulo = viewModel.titulo;
            maestrolinea.IdAreaacademica = viewModel.IdAreaacademica;
            maestrolinea.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestrolinea = await _context.MaestroLineas.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrolinea == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroLineas.Remove(maestrolinea);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestrolinea = await _context.MaestroLineas.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrolinea == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestrolinea.Id,
                maestrolinea.codigo,
                maestrolinea.titulo,
                maestrolinea.descripcion,
                maestrolinea.IdAreaacademica,
                maestrolinea.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroLinea, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.titulo;
                    break;
            }
          

            var query = _context.MaestroLineas
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.titulo.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)               
                .Select(x => new
                {
                    x.Id,
                    x.IdAreaacademica,
                    x.codigo,
                    x.titulo,
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
