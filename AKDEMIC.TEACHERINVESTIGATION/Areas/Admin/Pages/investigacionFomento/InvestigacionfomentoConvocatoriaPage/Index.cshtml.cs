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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriarequisitoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatorialistaverificacionViewModels;

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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoactividadViewModels;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;
using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFomento.InvestigacionfomentoConvocatoriaPage
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


        public IndexModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
           IOptions<CloudStorageCredentials> storageCredentials

        ) 
        {
            _context = context;
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;

        }
        public void OnGet()
        {
        }

        public InvestigacionfomentoConvocatoriaCreateViewModel fechaini { get; set; }
        
        public InvestigacionfomentoConvocatoriaCreateViewModel fechafin { get; set; }
        
        Guid xIdOficina = Guid.Parse("e0557aa7-6404-11ee-b7b1-16d13ee00159");

        public async Task<IActionResult> OnPostCreateAsync(InvestigacionfomentoConvocatoriaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoconvocatoria = new InvestigacionfomentoConvocatoria
            {
                IdOficina=xIdOficina,
                nombre = viewModel.nombre,
                IdCategoriaconvocatoria=viewModel.IdCategoriaconvocatoria,
                IdTipoconvocatoria=viewModel.IdTipoconvocatoria,
                IdFlujo=viewModel.IdFlujo,
                fechaini = ConvertHelpers.DatepickerToUtcDateTime(viewModel.fechaini),
                fechafin = ConvertHelpers.DatepickerToUtcDateTime(viewModel.fechafin),
                descripcion = viewModel.descripcion,
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionfomentoConvocatorias.AddAsync(investigacionfomentoconvocatoria);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionfomentoConvocatoriaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoconvocatoria = await _context.InvestigacionfomentoConvocatorias.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoria == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoconvocatoria.IdCategoriaconvocatoria = viewModel.IdCategoriaconvocatoria;
            investigacionfomentoconvocatoria.IdTipoconvocatoria = viewModel.IdTipoconvocatoria;
            investigacionfomentoconvocatoria.IdFlujo = viewModel.IdFlujo;
            investigacionfomentoconvocatoria.nombre = viewModel.nombre;
            investigacionfomentoconvocatoria.descripcion = viewModel.descripcion;
            investigacionfomentoconvocatoria.fechaini = ConvertHelpers.DatepickerToUtcDateTime(viewModel.fechaini);
            investigacionfomentoconvocatoria.fechafin = ConvertHelpers.DatepickerToUtcDateTime(viewModel.fechafin);
            investigacionfomentoconvocatoria.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionfomentoconvocatoria = await _context.InvestigacionfomentoConvocatorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoria == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoConvocatorias.Remove(investigacionfomentoconvocatoria);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionfomentoconvocatoria = await _context.InvestigacionfomentoConvocatorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoria == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentoconvocatoria.Id,
                investigacionfomentoconvocatoria.nombre,
               fechafin= DateTime.Parse(investigacionfomentoconvocatoria.fechafin.ToString()).ToString("dd/MM/yyyy"),
                fechaini= DateTime.Parse(investigacionfomentoconvocatoria.fechaini.ToString()).ToString("dd/MM/yyyy"),
                investigacionfomentoconvocatoria.IdCategoriaconvocatoria,
                investigacionfomentoconvocatoria.IdTipoconvocatoria,
                investigacionfomentoconvocatoria.IdFlujo,
                investigacionfomentoconvocatoria.descripcion,
                investigacionfomentoconvocatoria.activo ,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoria, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }

            var querycategoriaconvocatoria = _context.MaestroCategoriaconvocatorias.AsNoTracking();
            var querytipoconvocatoria = _context.MaestroTipoconvocatorias.AsNoTracking();
            var queryflujo = _context.InvestigacionfomentoFlujos.AsNoTracking();

            var query = _context.InvestigacionfomentoConvocatorias
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(x => (x.nombre.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                                   x.descripcion.ToLower().Trim().Contains(searchValue.ToLower().Trim())) && x.IdOficina == xIdOficina);

            }
            else
            {
                query = query.Where(x => x.IdOficina == xIdOficina);
            };
               
            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querycategoriaconvocatoria,
                    convocatoria1 => convocatoria1.IdCategoriaconvocatoria,
                    categoria1 => categoria1.Id,
                     (convocatoria1, categoria1) => new { convocatoria1, categoria1 })
                .Join(querytipoconvocatoria,
                    convocatoria2 => convocatoria2.convocatoria1.IdTipoconvocatoria,
                    tipo1 => tipo1.Id,
                     (convocatoria2, tipo1) => new { convocatoria2, tipo1 })
                .Join(queryflujo,
                    convocatoria3 => convocatoria3.convocatoria2.convocatoria1.IdFlujo,
                    flujo1 => flujo1.Id,
                     (convocatoria3, flujo1) => new { convocatoria3, flujo1 })
                .Select(x => new
                {
                   Id =  x.convocatoria3.convocatoria2.convocatoria1.Id,
                   nombre = x.convocatoria3.convocatoria2.convocatoria1.nombre,
                     fechaini = DateTime.Parse(x.convocatoria3.convocatoria2.convocatoria1.fechaini.ToString()).ToString("dd/MM/yyyy"),
                    fechafin = DateTime.Parse( x.convocatoria3.convocatoria2.convocatoria1.fechafin.ToString()).ToString("dd/MM/yyyy"),
                   nombrecategoria = x.convocatoria3.convocatoria2.categoria1.nombre,
                   nombretipo = x.convocatoria3.tipo1.nombre,
                   nombreflujo = x.flujo1.nombre,
                   estado = x.convocatoria3.convocatoria2.convocatoria1.activo,
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
        public async Task<IActionResult> OnGetDatatableArearequisitoAsync(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriarequisito, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.IdConvocatoria;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IdRequisito;
                    break;
            }


            var queryrequisito = _context.InvestigacionfomentoRequisitos.AsNoTracking();
            var queryarea = _context.MaestroAreas.AsNoTracking();

            var query = _context.InvestigacionfomentoConvocatoriarequisitos
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryrequisito,
                   convocatoriarequisito1 => convocatoriarequisito1.IdRequisito,
                   requisito1 => requisito1.Id,
                    (convocatoriarequisito1, requisito1) => new { convocatoriarequisito1, requisito1 })
                .Join(queryarea,
                convocatoriarequisito2 => convocatoriarequisito2.convocatoriarequisito1.IdArea,
                area1=>area1.Id,
                (convocatoriarequisito2, area1) => new { convocatoriarequisito2, area1 })
                .Select(x => new
                {
                    nombrearea = x.area1.nombre,
                    nombrerequisito =  x.convocatoriarequisito2.requisito1.nombre,
                    IdRequisito = x.convocatoriarequisito2.convocatoriarequisito1.IdRequisito,
                    IdArea = x.convocatoriarequisito2.convocatoriarequisito1.IdArea,
                    id = x.convocatoriarequisito2.convocatoriarequisito1.Id,
                    idconvocatoria = x.convocatoriarequisito2.convocatoriarequisito1.IdConvocatoria,
                    archivourl = x.convocatoriarequisito2.convocatoriarequisito1.archivourl??"",

                })
                .Where(x => x.idconvocatoria == id);

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

        public async Task<IActionResult> OnPostCreateArearequisitoAsync(InvestigacionfomentoConvocatoriarequisitoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var archivourl = "";
            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                archivourl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.CONVOCATORIA,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.INVESTIGACION_FOMENTO);
            }

            var investigacionfomentoconvocatoriarequisito = new InvestigacionfomentoConvocatoriarequisito
            {
                IdArea = viewModel.IdArea,
                IdRequisito = viewModel.IdRequisito,
                IdConvocatoria = viewModel.IdConvocatoria,
                archivourl = archivourl,
            };

            await _context.InvestigacionfomentoConvocatoriarequisitos.AddAsync(investigacionfomentoconvocatoriarequisito);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostDeletearearequisitoAsync(Guid id)
        {
            var investigacionfomentoareaarearequisito = await _context.InvestigacionfomentoConvocatoriarequisitos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoareaarearequisito == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoConvocatoriarequisitos.Remove(investigacionfomentoareaarearequisito);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailArearequisitoAsync(Guid id)
        {
            var investigacionfomentoareaarearequisitos = await _context.InvestigacionfomentoConvocatoriarequisitos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoareaarearequisitos == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentoareaarearequisitos.Id,
                investigacionfomentoareaarearequisitos.IdRequisito,
                investigacionfomentoareaarearequisitos.IdArea,
                investigacionfomentoareaarearequisitos.IdConvocatoria,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditArearequisitoAsync(InvestigacionfomentoConvocatoriarequisitoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoareaarearequisito = await _context.InvestigacionfomentoConvocatoriarequisitos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoareaarearequisito == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoareaarearequisito.IdArea = viewModel.IdArea;
            investigacionfomentoareaarearequisito.IdRequisito = viewModel.IdRequisito;

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnGetDatatableArealistaverificacionAsync(Guid id)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatorialistaverificacion, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.IdConvocatoria;
                    break;
                case "1":
                    orderByPredicate = (x) => x.IdListaverificacion;
                    break;
            }


            var querylistaverificacion = _context.InvestigacionfomentoListaverificaciones.AsNoTracking();
            var queryarea = _context.MaestroAreas.AsNoTracking();

            var query = _context.InvestigacionfomentoConvocatorialistaverificaciones
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querylistaverificacion,
                   convocatoriarequisito1 => convocatoriarequisito1.IdListaverificacion,
                   listaverificacion1 => listaverificacion1.Id,
                    (convocatoriarequisito1, listaverificacion1) => new { convocatoriarequisito1, listaverificacion1 })
                .Join(queryarea,
                convocatoriarequisito2 => convocatoriarequisito2.convocatoriarequisito1.IdArea,
                area1 => area1.Id,
                (convocatoriarequisito2, area1) => new { convocatoriarequisito2, area1 })
                .Select(x => new
                {
                    nombrearea = x.area1.nombre,
                    nombrelistaverificacion = x.convocatoriarequisito2.listaverificacion1.nombre,
                    IdListaverificacion1 = x.convocatoriarequisito2.convocatoriarequisito1.IdListaverificacion,
                    IdArea = x.convocatoriarequisito2.convocatoriarequisito1.IdArea,
                    id = x.convocatoriarequisito2.convocatoriarequisito1.Id,
                    idconvocatoria = x.convocatoriarequisito2.convocatoriarequisito1.IdConvocatoria,

                })
                .Where(x => x.idconvocatoria == id);

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

        public async Task<IActionResult> OnPostCreateArealistaverificacionAsync(InvestigacionfomentoConvocatorialistaverificacionCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

          

            var investigacionfomentoconvocatorialistaverificacion = new InvestigacionfomentoConvocatorialistaverificacion
            {
                IdArea = viewModel.IdArea,
                IdListaverificacion = viewModel.IdListaverificacion,
                IdConvocatoria = viewModel.IdConvocatoria,
            };

            await _context.InvestigacionfomentoConvocatorialistaverificaciones.AddAsync(investigacionfomentoconvocatorialistaverificacion);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnPostDeletearealistaverificacionAsync(Guid id)
        {
            var investigacionfomentoareaarealistaverificacion = await _context.InvestigacionfomentoConvocatorialistaverificaciones.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoareaarealistaverificacion == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoConvocatorialistaverificaciones.Remove(investigacionfomentoareaarealistaverificacion);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        
    }
}
