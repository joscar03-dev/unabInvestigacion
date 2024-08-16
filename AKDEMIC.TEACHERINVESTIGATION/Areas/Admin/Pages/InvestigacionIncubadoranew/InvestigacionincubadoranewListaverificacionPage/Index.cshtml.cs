using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Options;

using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoListaverificacionViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoListaverificacionindicadorViewModels;


using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Auth;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using AKDEMIC.DOMAIN.Entities.General;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Web;


namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionIncubadoranew.InvestigacionIncubadoranewListaverificacionPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly UserManager<ApplicationUser> _userManager;



        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials,
            UserManager<ApplicationUser> userManager

        )
        {
            _userManager = userManager;
            _storageCredentials = storageCredentials;
            _context = context;
            _dataTablesService = dataTablesService;
        }
        public void OnGet()
        {
        }
        Guid xIdOficina = Guid.Parse("5628b2c0-9a3d-11ee-b7b1-16d13ee00159");

        public async Task<IActionResult> OnPostCreateAsync(InvestigacionfomentoListaverificacionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoflujo = new InvestigacionfomentoListaverificacion
            {
                IdOficina = xIdOficina,
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionfomentoListaverificaciones.AddAsync(investigacionfomentoflujo);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionfomentoListaverificacionEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoflujo = await _context.InvestigacionfomentoListaverificaciones.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoflujo == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoflujo.codigo = viewModel.codigo;
            investigacionfomentoflujo.descripcion = viewModel.descripcion;
            investigacionfomentoflujo.nombre = viewModel.nombre;
            investigacionfomentoflujo.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionfomentoflujo = await _context.InvestigacionfomentoListaverificaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoflujo == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoListaverificaciones.Remove(investigacionfomentoflujo);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionfomentoflujo = await _context.InvestigacionfomentoListaverificaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoflujo == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentoflujo.Id,
                investigacionfomentoflujo.codigo,
                investigacionfomentoflujo.nombre,
                investigacionfomentoflujo.descripcion,
                investigacionfomentoflujo.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoListaverificacion, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.InvestigacionfomentoListaverificaciones
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

        public async Task<IActionResult> OnGetDatatableIndicadorAsync(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoListaverificacionindicador, dynamic>> orderByPredicate = null;

            var user = await _userManager.GetUserAsync(User);

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.activo;
                    break;
                case "1":
                    orderByPredicate = (x) => x.activo;
                    break;
            }
            

            var queryareas = _context.InvestigacionfomentoIndicadores.AsNoTracking();

            var query = _context.InvestigacionfomentoListaverificacionindicadores
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryareas,
                   listaverificacion1 => listaverificacion1.IdIndicador,
                   indicador1 => indicador1.Id,
                    (listaverificacion1, indicador1) => new { listaverificacion1, indicador1 })
                .Select(x => new
                {
                    nombre = x.indicador1.nombre,
                    IdIndicador = x.listaverificacion1.IdIndicador,
                    id = x.listaverificacion1.Id,
                    IdListaverificacion = x.listaverificacion1.IdListaverificacion,
                    activo = x.listaverificacion1.activo,

                })
                .Where(x => x.IdListaverificacion == id);

            int recordsFiltered = await query.CountAsync();


            var data = await query.Skip(sentParameters.PagingFirstRecord)
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
        public async Task<IActionResult> OnPostCreateIndicadorAsync(InvestigacionfomentoListaverificacionindicadorCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentolistaverificacionindicador = new InvestigacionfomentoListaverificacionindicador
            {
                IdIndicador = viewModel.IdIndicador,
                IdListaverificacion = viewModel.IdListaverificacion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionfomentoListaverificacionindicadores.AddAsync(investigacionfomentolistaverificacionindicador);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostDeleteindicadorAsync(Guid id)
        {
            var investigacionfomentolistaverificacionindicador = await _context.InvestigacionfomentoListaverificacionindicadores.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentolistaverificacionindicador == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoListaverificacionindicadores.Remove(investigacionfomentolistaverificacionindicador);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailIndicadorAsync(Guid id)
        {
            var investigacionfomentolistaverificacionindicador = await _context.InvestigacionfomentoListaverificacionindicadores.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentolistaverificacionindicador == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentolistaverificacionindicador.Id,
                investigacionfomentolistaverificacionindicador.IdListaverificacion,
                investigacionfomentolistaverificacionindicador.IdIndicador,
                investigacionfomentolistaverificacionindicador.activo,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditIndicadorAsync(InvestigacionfomentoListaverificacionindicadorEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentolistaverificacionindicador = await _context.InvestigacionfomentoListaverificacionindicadores.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentolistaverificacionindicador == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentolistaverificacionindicador.IdIndicador = viewModel.IdIndicador;
            investigacionfomentolistaverificacionindicador.IdListaverificacion = viewModel.IdListaverificacion;
            investigacionfomentolistaverificacionindicador.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

    }
}
