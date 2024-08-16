using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesorViewModels;


namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.InvestigacionasesoriaAsesorPage
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


        public async Task<IActionResult> OnPostCreateAsync(InvestigacionasesoriaAsesorCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrodocente = new InvestigacionasesoriaAsesor
            {
                IdUser = viewModel.IdUser,
                IdCarrera = viewModel.IdCarrera,
              
            };

            await _context.InvestigacionasesoriaAsesores.AddAsync(maestrodocente);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionasesoriaAsesorEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrodocente = await _context.InvestigacionasesoriaAsesores.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestrodocente == null) return new BadRequestObjectResult("Sucedio un error");

            maestrodocente.IdCarrera = viewModel.IdCarrera;
            maestrodocente.IdUser = viewModel.IdUser;
          
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestrodocente = await _context.InvestigacionasesoriaAsesores.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrodocente == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionasesoriaAsesores.Remove(maestrodocente);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestrodocente = await _context.InvestigacionasesoriaAsesores.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrodocente == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestrodocente.Id,
                maestrodocente.IdUser,
                maestrodocente.IdCarrera,
               
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionasesoriaAsesor, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IdCarrera;
                    break;
            }

            var querycarreras = _context.MaestroCarreras.AsNoTracking();
          
            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();
         

            var query = _context.InvestigacionasesoriaAsesores
                .AsNoTracking();

            
            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querycarreras, 
                    asesor1 => asesor1.IdCarrera,                    
                    carrera1 => carrera1.Id,
                     (asesor1, carrera1) => new  { asesor1, carrera1 }       )        
                .Join(queryusuarios,
                    asesor2 => asesor2.asesor1.IdUser,
                    user1 => user1.Id,
                    (asesor2, user1) => new { asesor2, user1 })
                
                .Select(x => new
                {
                    nombrecarrera = x.asesor2.carrera1.nombre,                  
                    Id = x.asesor2.asesor1.Id,
                    nombreuser = x.user1.FullName,
                })





                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();

            int recordsTotal = data.Count;
            int i=0;
           
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