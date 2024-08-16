using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.MaestroAlumnoViewModels;
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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.MaestroAlumnoPage
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


        public async Task<IActionResult> OnPostCreateAsync(MaestroAlumnoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroalumno = new MaestroAlumno
            {
                IdUser = viewModel.IdUser,
                IdCarrera = viewModel.IdCarrera,
                codigo = viewModel.codigo,               
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroAlumnos.AddAsync(maestroalumno);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroAlumnoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroalumno = await _context.MaestroAlumnos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestroalumno == null) return new BadRequestObjectResult("Sucedio un error");

            maestroalumno.IdCarrera = viewModel.IdCarrera;
            maestroalumno.IdUser = viewModel.IdUser;
            maestroalumno.codigo = viewModel.codigo;           
            maestroalumno.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestroalumno = await _context.MaestroAlumnos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroalumno == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroAlumnos.Remove(maestroalumno);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestroalumno = await _context.MaestroAlumnos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroalumno == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestroalumno.Id,
                maestroalumno.IdUser,
                maestroalumno.IdCarrera,
                maestroalumno.codigo,
              
                maestroalumno.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroAlumno, dynamic>> orderByPredicate = null;

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

            var query = _context.MaestroAlumnos
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querycarreras, 
                    alumnos1 => alumnos1.IdCarrera,                    
                    carrera1 => carrera1.Id,
                     (alumnos1, carrera1) => new  { alumnos1, carrera1 })                
                .Join(queryusuarios,
                    alumnos2 => alumnos2.alumnos1.IdUser,
                    user1 => user1.Id,
                    (alumnos2, user1) => new { alumnos2, user1 })                 
                .Select(x => new
                {
                    nombrecarrera = x.alumnos2.carrera1.nombre,
                    activo = x.alumnos2.alumnos1.activo,
                    Id = x.alumnos2.alumnos1.Id,
                    nombreuser = x.user1.FullName,
                    codigo = x.alumnos2.alumnos1.codigo,
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