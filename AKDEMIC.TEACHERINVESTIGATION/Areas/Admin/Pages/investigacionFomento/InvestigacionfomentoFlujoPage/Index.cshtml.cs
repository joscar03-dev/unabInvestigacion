using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Options;

using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoFlujoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoFlujosareaViewModels;


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


namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFomento.InvestigacionfomentoFlujoPage
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
        Guid xIdOficina = Guid.Parse("e0557aa7-6404-11ee-b7b1-16d13ee00159");


        public async Task<IActionResult> OnPostCreateAsync(InvestigacionfomentoFlujoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoflujo = new InvestigacionfomentoFlujo
            {
                IdOficina=xIdOficina,
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionfomentoFlujos.AddAsync(investigacionfomentoflujo);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionfomentoFlujoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoflujo = await _context.InvestigacionfomentoFlujos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

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
            var investigacionfomentoflujo = await _context.InvestigacionfomentoFlujos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoflujo == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoFlujos.Remove(investigacionfomentoflujo);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionfomentoflujo = await _context.InvestigacionfomentoFlujos.Where(x => x.Id == id).FirstOrDefaultAsync();

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
            Expression<Func<InvestigacionfomentoFlujo, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.InvestigacionfomentoFlujos
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

        public async Task<IActionResult> OnGetDatatableAreaAsync(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoFlujosarea, dynamic>> orderByPredicate = null;

            var user = await _userManager.GetUserAsync(User);

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.orden;
                    break;
                case "1":
                    orderByPredicate = (x) => x.orden;
                    break;
            }


            var queryareas = _context.MaestroAreas.AsNoTracking();

            var query = _context.InvestigacionfomentoFlujosareas
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryareas,
                   flujoarea1 => flujoarea1.IdArea,
                   area1 => area1.Id,
                    (flujoarea1, area1) => new { flujoarea1, area1 })
                .Select(x => new
                {
                    nombre = x.area1.nombre,
                    IdArea = x.flujoarea1.IdArea,
                    id = x.flujoarea1.Id,
                    orden = x.flujoarea1.orden,
                    IdFlujo = x.flujoarea1.IdFlujo,
                    activo = x.flujoarea1.activo,
                    retornadocente =  x.flujoarea1.retornadocente,

                })
                .Where(x => x.IdFlujo == id)
                .OrderBy(x=> x.orden);

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
        public async Task<IActionResult> OnPostCreateAreaAsync(InvestigacionfomentoFlujosareaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoflujosarea = new InvestigacionfomentoFlujosarea
            {
                IdArea = viewModel.IdArea,
                IdFlujo = viewModel.IdFlujo,
                orden = viewModel.orden,
                activo = viewModel.activo ?? "0",
                retornadocente = viewModel.retornadocente ?? "0",
            };

            await _context.InvestigacionfomentoFlujosareas.AddAsync(investigacionfomentoflujosarea);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostDeleteareaAsync(Guid id)
        {
            var investigacionfomentoflujosarea = await _context.InvestigacionfomentoFlujosareas.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoflujosarea == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoFlujosareas.Remove(investigacionfomentoflujosarea);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailAreaAsync(Guid id)
        {
            var investigacionfomentoflujosarea = await _context.InvestigacionfomentoFlujosareas.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoflujosarea == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentoflujosarea.Id,
                investigacionfomentoflujosarea.IdFlujo,
                investigacionfomentoflujosarea.IdArea,
                investigacionfomentoflujosarea.orden,
                investigacionfomentoflujosarea.activo,
                investigacionfomentoflujosarea.retornadocente,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditAreaAsync(InvestigacionfomentoFlujosareaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoflujosarea = await _context.InvestigacionfomentoFlujosareas.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoflujosarea == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoflujosarea.IdArea = viewModel.IdArea;
            investigacionfomentoflujosarea.orden = viewModel.orden;
            investigacionfomentoflujosarea.activo = viewModel.activo ?? "0";
            investigacionfomentoflujosarea.retornadocente = viewModel.retornadocente;
            await _context.SaveChangesAsync();

            return new OkResult();
        }

    }
}
