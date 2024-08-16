using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroDocenteViewModels;
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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.MaestroDocentePage
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


        public async Task<IActionResult> OnPostCreateAsync(MaestroDocenteCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrodocente = new MaestroDocente
            {
                IdUser = viewModel.IdUser,
                IdDepartamento = viewModel.IdDepartamento,
                IdTipogrado = viewModel.IdTipogrado,
                IdFacultad = viewModel.IdFacultad,
                IdCategoriadocente =  viewModel.IdCategoriadocente,
                perfil=viewModel.perfil,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroDocentes.AddAsync(maestrodocente);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroDocenteEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestrodocente = await _context.MaestroDocentes.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestrodocente == null) return new BadRequestObjectResult("Sucedio un error");

            maestrodocente.IdDepartamento = viewModel.IdDepartamento;
            maestrodocente.IdUser = viewModel.IdUser;
            maestrodocente.IdTipogrado = viewModel.IdTipogrado;
            maestrodocente.perfil = viewModel.perfil;
            maestrodocente.IdFacultad = viewModel.IdFacultad;
            maestrodocente.IdCategoriadocente = viewModel.IdCategoriadocente;
            maestrodocente.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestrodocente = await _context.MaestroDocentes.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrodocente == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroDocentes.Remove(maestrodocente);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestrodocente = await _context.MaestroDocentes.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestrodocente == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestrodocente.Id,
                maestrodocente.IdUser,
                maestrodocente.IdDepartamento,
                maestrodocente.IdCategoriadocente,
                maestrodocente.IdTipogrado,
                maestrodocente.perfil,
                maestrodocente.IdFacultad,
                maestrodocente.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroDocente, dynamic>> orderByPredicate = null;
           
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
            var querydepartamentos = _context.MaestroDepartamentos.AsNoTracking();
            var querytipogrados = _context.MaestroTipogrados.AsNoTracking();
            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();
            var querycategoriadocentes = _context.MaestroCategoriadocentes.AsNoTracking();

            var query = _context.MaestroDocentes
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.perfil.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.perfil.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryfacultades, 
                    docentes1 => docentes1.IdFacultad,                    
                    facultad1 => facultad1.Id,
                     (docentes1, facultad1) => new  { docentes1, facultad1 })
                .Join(querydepartamentos,
                    docentes2 => docentes2.docentes1.IdDepartamento,
                    departamento1 => departamento1.Id,
                    (docentes2, departamento1) => new  { docentes2, departamento1 })
                .Join(querytipogrados,
                    docentes3 => docentes3.docentes2.docentes1.IdTipogrado,
                    tipogrados1 => tipogrados1.Id,
                    (docentes3 , tipogrados1) => new { docentes3, tipogrados1 })
                .Join(queryusuarios,
                    docentes4 => docentes4.docentes3.docentes2.docentes1.IdUser,
                    user1 => user1.Id,
                    (docentes4, user1) => new { docentes4, user1 })
                 .Join(querycategoriadocentes,
                    docentes5 => docentes5.docentes4.docentes3.docentes2.docentes1.IdCategoriadocente,
                    categoriadocente1 => categoriadocente1.Id,
                    (docentes5, categoriadocente1) => new { docentes5, categoriadocente1 })
                .Select(x => new
                {
                    nombredepartamento= x.docentes5.docentes4.docentes3.departamento1.nombre,
                    nombrefacultad = x.docentes5.docentes4.docentes3.docentes2.facultad1.nombre,
                    nombretipogrado = x.docentes5.docentes4.tipogrados1.nombre,
                    activo = x.docentes5.docentes4.docentes3.docentes2.docentes1.activo,
                    Id = x.docentes5.docentes4.docentes3.docentes2.docentes1.Id,
                    nombreuser = x.docentes5.user1.FullName,
                    nombrecategoriadocente = x.categoriadocente1.nombre,
                    codigo =x.docentes5.docentes4.docentes3.docentes2.docentes1.codigo,
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