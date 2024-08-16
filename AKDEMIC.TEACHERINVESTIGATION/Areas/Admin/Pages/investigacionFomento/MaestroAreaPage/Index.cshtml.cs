using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Options;
using System.IO;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.MaestroAreaViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.MaestroAreasusuarioViewModels;


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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFomento.MaestroAreaPage
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

        public async Task<IActionResult> OnPostCreateAsync(MaestroAreaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroarea = new MaestroArea
            {
                IdOficina= xIdOficina,
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroAreas.AddAsync(maestroarea);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(MaestroAreaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroarea = await _context.MaestroAreas.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestroarea == null) return new BadRequestObjectResult("Sucedio un error");

            maestroarea.codigo = viewModel.codigo;
            maestroarea.descripcion = viewModel.descripcion;
            maestroarea.nombre = viewModel.nombre;
            maestroarea.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var maestroarea = await _context.MaestroAreas.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroarea == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroAreas.Remove(maestroarea);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var maestroarea = await _context.MaestroAreas.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroarea == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestroarea.Id,
                maestroarea.codigo,
                maestroarea.nombre,
                maestroarea.descripcion,
                maestroarea.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroArea, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.MaestroAreas
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => (x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                                  x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim())) && x.IdOficina == xIdOficina);

            }
            else
            {
                query = query.Where(x => x.IdOficina == xIdOficina);


            }
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

        public async Task<IActionResult> OnGetDatatableUsuariosAsync(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<MaestroAreasusuario, dynamic>> orderByPredicate = null;

            var user = await _userManager.GetUserAsync(User);

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.IdUser;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IdArea;
                    break;
            }


            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();

            var query = _context.MaestroAreasusuarios
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryusuarios,
                   areausuario1 => areausuario1.IdUser,
                   usuario1 => usuario1.Id,
                    (areausuario1, usuario1) => new { areausuario1, usuario1 })
                .Select(x => new
                {
                    fullName = x.usuario1.FullName,
                    IdArea =  x.areausuario1.IdArea,
                    id = x.areausuario1.Id,
                    IdUser =  x.areausuario1.IdUser,
                    activo=x.areausuario1.activo,

                })
                .Where(x => x.IdArea == id);

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
        public async Task<IActionResult> OnPostCreateUsuarioAsync(MaestroAreasusuarioCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroareasusuario = new MaestroAreasusuario
            {
                IdArea = viewModel.IdArea,
                IdUser = viewModel.IdUser,
                activo = viewModel.activo ?? "0",
            };

            await _context.MaestroAreasusuarios.AddAsync(maestroareasusuario);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostDeleteusuarioAsync(Guid id)
        {
            var maestroareausuario = await _context.MaestroAreasusuarios.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroareausuario == null) return new BadRequestObjectResult("Sucedio un error");

            _context.MaestroAreasusuarios.Remove(maestroareausuario);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailUsuarioAsync(Guid id)
        {
            var maestroareausuarios = await _context.MaestroAreasusuarios.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (maestroareausuarios == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                maestroareausuarios.Id,
                maestroareausuarios.IdUser,
                maestroareausuarios.IdArea,
                maestroareausuarios.activo,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditUsuarioAsync(MaestroAreasusuarioEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var maestroareausuario = await _context.MaestroAreasusuarios.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (maestroareausuario == null) return new BadRequestObjectResult("Sucedio un error");

            maestroareausuario.IdUser = viewModel.IdUser;
            maestroareausuario.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

    }
}
