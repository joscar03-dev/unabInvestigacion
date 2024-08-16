using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaConfiguracionanioViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.MaestroAreaacademicaViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.UserViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.InvestigacionformativaConfiguraranioPage
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

        public InvestigacionformativaConfiguracionanioCreateViewModel fechaini { get; set; }
        public InvestigacionformativaConfiguracionanioCreateViewModel fechafin { get; set; }

        public async Task<IActionResult> OnPostCreateAsync(InvestigacionformativaConfiguracionanioCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var query = _context.InvestigacionformativaConfiguracionanios.AsNoTracking();

            foreach (var item in query)
            {
                Console.WriteLine(item.Id);
                var query2= await _context.InvestigacionformativaConfiguracionanios.Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                query2.activo = "0";
                await _context.SaveChangesAsync();

            }

            DateTime? fechaini = null;
            DateTime? fechafin = null;
            if (!string.IsNullOrEmpty(viewModel.fechaini))
            {
                fechaini = ConvertHelpers.DatepickerToDatetime(viewModel.fechaini);
            }

            if (!string.IsNullOrEmpty(viewModel.fechafin))
            {
                fechafin = ConvertHelpers.DatepickerToDatetime(viewModel.fechafin);
            }

            var investigacionformativaconfiguracionanio = new InvestigacionformativaConfiguracionanio
            {
                nombre = viewModel.nombre,
                fechaini = fechaini,
                fechafin = fechafin,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionformativaConfiguracionanios.AddAsync(investigacionformativaconfiguracionanio);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionformativaConfiguracionanioEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var query = _context.InvestigacionformativaConfiguracionanios.AsNoTracking();

            foreach (var item in query)
            {
                Console.WriteLine(item.Id);
                var query2 = await _context.InvestigacionformativaConfiguracionanios.Where(x => x.Id == item.Id).FirstOrDefaultAsync();
                query2.activo = "0";
                await _context.SaveChangesAsync();

            }

            var investigacionformativaconfiguracionanio = await _context.InvestigacionformativaConfiguracionanios.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaconfiguracionanio == null) return new BadRequestObjectResult("Sucedio un error");

            DateTime? fechaini = null;
            DateTime? fechafin = null;
            if (!string.IsNullOrEmpty(viewModel.fechaini.ToString()))
            {
                fechaini = ConvertHelpers.DatepickerToDatetime(viewModel.fechaini.ToString());
            }

            if (!string.IsNullOrEmpty(viewModel.fechafin.ToString()))
            {
                fechafin = ConvertHelpers.DatepickerToDatetime(viewModel.fechafin.ToString());
            }
            investigacionformativaconfiguracionanio.descripcion = viewModel.descripcion;
            investigacionformativaconfiguracionanio.nombre = viewModel.nombre;
            investigacionformativaconfiguracionanio.activo = viewModel.activo ?? "0";
            investigacionformativaconfiguracionanio.fechaini = fechaini;
            investigacionformativaconfiguracionanio.fechafin = fechafin;
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        
        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionformativaconfiguracionanio = await _context.InvestigacionformativaConfiguracionanios.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaconfiguracionanio == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionformativaConfiguracionanios.Remove(investigacionformativaconfiguracionanio);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionformativaconfiguracionanio = await _context.InvestigacionformativaConfiguracionanios.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaconfiguracionanio == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionformativaconfiguracionanio.Id,
                fechaini = DateTime.Parse(investigacionformativaconfiguracionanio.fechaini.ToString()).ToString("dd/MM/yyyy"),
                fechafin = DateTime.Parse(investigacionformativaconfiguracionanio.fechafin.ToString()).ToString("dd/MM/yyyy"),
              
                investigacionformativaconfiguracionanio.nombre,
                investigacionformativaconfiguracionanio.descripcion,
                investigacionformativaconfiguracionanio.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionformativaConfiguracionanio, dynamic>> orderByPredicate = null;
           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }
          

            var query = _context.InvestigacionformativaConfiguracionanios
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.descripcion.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    x.Id,
                    fechaini=DateTime.Parse(x.fechaini.ToString()).ToString("dd/MM/yyyy"),
                    fechafin = DateTime.Parse(x.fechafin.ToString()).ToString("dd/MM/yyyy"),
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
