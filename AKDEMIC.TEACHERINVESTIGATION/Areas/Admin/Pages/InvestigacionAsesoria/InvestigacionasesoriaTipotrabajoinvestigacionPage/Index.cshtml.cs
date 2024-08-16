using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Options;
using System.IO;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaTipotrabajoinvestigacionViewModels;


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

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.InvestigacionasesoriaTipotrabajoinvestigacionPage
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


        public async Task<IActionResult> OnPostCreateAsync(InvestigacionasesoriaTipotrabajoinvestigacionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionasesoriatipotrabajoinvestigacion = new InvestigacionasesoriaTipotrabajoinvestigacion
            {
                codigo = viewModel.codigo,
                nombre = viewModel.nombre,
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionasesoriaTipotrabajoinvestigaciones.AddAsync(investigacionasesoriatipotrabajoinvestigacion);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionasesoriaTipotrabajoinvestigacionEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionasesoriatipotrabajoinvestigacion = await _context.InvestigacionasesoriaTipotrabajoinvestigaciones.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionasesoriatipotrabajoinvestigacion == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionasesoriatipotrabajoinvestigacion.codigo = viewModel.codigo;
            investigacionasesoriatipotrabajoinvestigacion.descripcion = viewModel.descripcion;
            investigacionasesoriatipotrabajoinvestigacion.nombre = viewModel.nombre;
            investigacionasesoriatipotrabajoinvestigacion.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionasesoriatipotrabajoinvestigacion = await _context.InvestigacionasesoriaTipotrabajoinvestigaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriatipotrabajoinvestigacion == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionasesoriaTipotrabajoinvestigaciones.Remove(investigacionasesoriatipotrabajoinvestigacion);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionasesoriatipotrabajoinvestigacion = await _context.InvestigacionasesoriaTipotrabajoinvestigaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriatipotrabajoinvestigacion == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionasesoriatipotrabajoinvestigacion.Id,
                investigacionasesoriatipotrabajoinvestigacion.codigo,
                investigacionasesoriatipotrabajoinvestigacion.nombre,
                investigacionasesoriatipotrabajoinvestigacion.descripcion,
                investigacionasesoriatipotrabajoinvestigacion.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionasesoriaTipotrabajoinvestigacion, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.InvestigacionasesoriaTipotrabajoinvestigaciones
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.codigo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

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

      
    }
}
