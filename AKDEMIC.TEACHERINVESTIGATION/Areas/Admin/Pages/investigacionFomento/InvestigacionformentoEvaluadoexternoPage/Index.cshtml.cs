using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;

using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoEvaluadorexternoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaComiteViewModels;
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
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using static ClosedXML.Excel.XLPredefinedFormat;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.investigacionFomento.InvestigacionformentoEvaluadorexternoPage
{
    [Authorize(Roles = GeneralConstants.ROLES.EVALUATOR_COMMITTEE + "," +
        GeneralConstants.ROLES.SUPERADMIN + "," +
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


        public async Task<IActionResult> OnPostCreateAsync(InvestigacionfomentoEvaluadorexternoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var InvestigacionfomentoEvaluadorexterno = new InvestigacionfomentoEvaluadorexterno
            {
                IdUser = viewModel.IdUser,             
                IdFacultad = viewModel.IdFacultad,
                descripcion=viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionfomentoEvaluadorexternos.AddAsync(InvestigacionfomentoEvaluadorexterno);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionfomentoEvaluadorexternoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var InvestigacionfomentoEvaluadorexterno = await _context.InvestigacionfomentoEvaluadorexternos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (InvestigacionfomentoEvaluadorexterno == null) return new BadRequestObjectResult("Sucedio un error");

            InvestigacionfomentoEvaluadorexterno.IdUser = viewModel.IdUser;
            InvestigacionfomentoEvaluadorexterno.descripcion = viewModel.descripcion;
            InvestigacionfomentoEvaluadorexterno.IdFacultad = viewModel.IdFacultad;
            InvestigacionfomentoEvaluadorexterno.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var InvestigacionfomentoEvaluadorexterno = await _context.InvestigacionfomentoEvaluadorexternos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (InvestigacionfomentoEvaluadorexterno == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoEvaluadorexternos.Remove(InvestigacionfomentoEvaluadorexterno);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var InvestigacionfomentoEvaluadorexterno = await _context.InvestigacionfomentoEvaluadorexternos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (InvestigacionfomentoEvaluadorexterno == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                InvestigacionfomentoEvaluadorexterno.Id,
                InvestigacionfomentoEvaluadorexterno.IdUser,
                InvestigacionfomentoEvaluadorexterno.descripcion,
                InvestigacionfomentoEvaluadorexterno.IdFacultad,
                InvestigacionfomentoEvaluadorexterno.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoEvaluadorexterno, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IdFacultad;
                    break;
            }

            var queryfacultades = _context.MaestroFacultades.AsNoTracking();            
            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();

            var query = _context.InvestigacionfomentoEvaluadorexternos.AsNoTracking();

          
            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryfacultades, 
                    docentes1 => docentes1.IdFacultad,                    
                    facultad1 => facultad1.Id,
                     (docentes1, facultad1) => new  { docentes1, facultad1 })                
                .Join(queryusuarios,
                    docentes2 => docentes2.docentes1.IdUser,
                    user1 => user1.Id,
                    (docentes2, user1) => new { docentes2, user1 })
                .Select(x => new
                {
                    nombrefacultad = x.docentes2.facultad1.nombre,
                    activo = x.docentes2.docentes1.activo,
                    Id = x.docentes2.docentes1.Id,
                    nombreuser = x.user1.FullName,
                })

                /*if (!string.IsNullOrEmpty(searchValue))
              query = query.Where(x => x.descipcion.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                  x.descipcion.ToLower().Trim().Contains(searchValue.ToLower().Trim()));
          */



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