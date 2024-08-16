using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectomiembroViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividadViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectopresupuestoViewModels;

using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividaddetalleViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectorequisitoViewModels;
using Microsoft.WindowsAzure.Storage.Auth;
using AKDEMIC.CORE.Options;
using Microsoft.Extensions.Options;


using System.IO;







namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionInnovacion.InvestigacioninnovacionConvocatoriaproyectoPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," + 
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," + GeneralConstants.ROLES.PUBLICATION_UNIT)]
    public class DetailModel : PageModel
    {

        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;

        public DetailModel(
            AkdemicContext context,
            IDataTablesService dataTablesService,
            UserManager<ApplicationUser> userManager,
            IOptions<CloudStorageCredentials> storageCredentials
        )
        {
            _context = context;
            _userManager = userManager;
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;
        }


        public async Task<IActionResult> OnGet(Guid id)
        {
            var lista = new List<InvestigacionfomentoConvocatoriaproyecto>();
            lista = _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == id).ToList();
            ViewData["ListaProyecto"] = lista;
            ViewData["id"] = id;
            return Page();
         
        }
        //Datatable de Autor


        public async Task<IActionResult> OnPostEditProyectoAsync(InvestigacionfomentoConvocatoriaproyectoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoconvocatoriaproyecto = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();
            Console.WriteLine(viewModel.antecedente);
            if (investigacionfomentoconvocatoriaproyecto == null) return new BadRequestObjectResult("Sucedio un error");

            if (viewModel.tipo == "1")
            {
                investigacionfomentoconvocatoriaproyecto.problema = viewModel.problema;

            }
            if (viewModel.tipo == "2")
            {
                investigacionfomentoconvocatoriaproyecto.antecedente = viewModel.antecedente;

            }
            if (viewModel.tipo == "3")
            {
                investigacionfomentoconvocatoriaproyecto.resultado = viewModel.resultado;

            }
            if (viewModel.tipo == "4")
            {
                investigacionfomentoconvocatoriaproyecto.justificacion = viewModel.justificacion;

            }
            if (viewModel.tipo == "5")
            {
                investigacionfomentoconvocatoriaproyecto.hipotesis = viewModel.hipotesis;

            }
            if (viewModel.tipo == "6")
            {
                investigacionfomentoconvocatoriaproyecto.preguntas = viewModel.preguntas;

            }
            if (viewModel.tipo == "7")
            {
                investigacionfomentoconvocatoriaproyecto.objetivogeneral = viewModel.objetivogeneral;

            }
            if (viewModel.tipo == "8")
            {
                investigacionfomentoconvocatoriaproyecto.objetivoespecifico = viewModel.objetivoespecifico;

            }
            if (viewModel.tipo == "9")
            {
                investigacionfomentoconvocatoriaproyecto.metodologia = viewModel.metodologia;

            }
            if (viewModel.tipo == "10")
            {
                investigacionfomentoconvocatoriaproyecto.riesgos = viewModel.riesgos;

            }

            if (viewModel.tipo == "11")
            {
                investigacionfomentoconvocatoriaproyecto.resumencientifico = viewModel.resumencientifico;

            }
            if (viewModel.tipo == "12")
            {
                investigacionfomentoconvocatoriaproyecto.equipamiento = viewModel.equipamiento;

            }
            if (viewModel.tipo == "13")
            {
                investigacionfomentoconvocatoriaproyecto.resultados = viewModel.resultados;

            }
            if (viewModel.tipo == "14")
            {
                investigacionfomentoconvocatoriaproyecto.sostenibilidad = viewModel.sostenibilidad;

            }
            if (viewModel.tipo == "15")
            {
                investigacionfomentoconvocatoriaproyecto.impacto = viewModel.impacto;

            }
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailPlanActividadesAsync(Guid id)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriaproyectoactividad, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.nombremes;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombremes;
                    break;
            }


            var queryproyectocronograma = _context.InvestigacionfomentoConvocatoriaproyectoscronogramas
                .Where(x => x.IdConvocatoriaproyecto == id)
                .AsNoTracking();

            var queryproyectoactividaddetalle = _context.InvestigacionfomentoConvocatoriaproyectosactividadesdetalles
                .Join(queryproyectocronograma,
                proyectoactividaddetalle1 => proyectoactividaddetalle1.IdConvocatoriaproyectocronograma,
                proyectocronograma1 => proyectocronograma1.Id,
                (proyectoactividaddetalle1, proyectocronograma1) => new { proyectoactividaddetalle1, proyectocronograma1 })
                .GroupBy(x => x.proyectoactividaddetalle1.IdConvocatoriaproyectoactividad)

                .Select(x => new
                {
                    id = x.Key,
                    Text = String.Join(",", x.Select(y => y.proyectocronograma1.nombremes))

                }).AsNoTracking();


            var query = _context.InvestigacionfomentoConvocatoriaproyectosactividades.AsNoTracking();
            int recordsFiltered = await query.CountAsync();
            var data = await query
            .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            .GroupJoin(queryproyectoactividaddetalle,
            convocatoriapproyectoactividad1 => convocatoriapproyectoactividad1.Id,
            convocatoriaproyectoacrividasdetalle1 => convocatoriaproyectoacrividasdetalle1.id,
            (convocatoriapproyectoactividad1, convocatoriaproyectoacrividasdetalle1) => new { convocatoriapproyectoactividad1, convocatoriaproyectoacrividasdetalle1 })
            .SelectMany(x => x.convocatoriaproyectoacrividasdetalle1.DefaultIfEmpty(),
                        (convocatoriapproyectoactividad2, convocatoriaproyectoacrividasdetalle2) => new { convocatoriapproyectoactividad2, convocatoriaproyectoacrividasdetalle2 }
                    )
            .Select(x => new
            {
                x.convocatoriapproyectoactividad2.convocatoriapproyectoactividad1.IdConvocatoriaproyecto,
                x.convocatoriapproyectoactividad2.convocatoriapproyectoactividad1.Id,
                x.convocatoriapproyectoactividad2.convocatoriapproyectoactividad1.titulo,
                x.convocatoriapproyectoactividad2.convocatoriapproyectoactividad1.estado,
                archivourl = x.convocatoriapproyectoactividad2.convocatoriapproyectoactividad1.archivourl ?? "",
                nombremeses = x.convocatoriaproyectoacrividasdetalle2.Text ?? "",

            })
            .Where(x => x.IdConvocatoriaproyecto == id)
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
        public async Task<IActionResult> OnPostNewActividadAsync(InvestigacionfomentoConvocatoriaproyectoactividadCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");


            var investigacionfomentoconvocatoriaproyectoactividad = new InvestigacionfomentoConvocatoriaproyectoactividad
            {
                titulo = viewModel.titulo,
                IdConvocatoriaproyecto = viewModel.IdConvocatoriaproyecto,
                estado = "0"
            };

            await _context.InvestigacionfomentoConvocatoriaproyectosactividades.AddAsync(investigacionfomentoconvocatoriaproyectoactividad);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnGetDetailPlanCronogramasAsync(Guid id)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriaproyectoactividaddetalle, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.Id;
                    break;
            }


            var queryconograma = _context.InvestigacionfomentoConvocatoriaproyectoscronogramas.AsNoTracking();
            var query = _context.InvestigacionfomentoConvocatoriaproyectosactividadesdetalles.AsNoTracking();
            int recordsFiltered = await query.CountAsync();
            var data = await query
            .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
            .Join(queryconograma,
            actividaddetalle1 => actividaddetalle1.IdConvocatoriaproyectocronograma,
            cronograma1 => cronograma1.Id,
            (actividaddetalle1, cronograma1) => new { actividaddetalle1, cronograma1 })
            .Select(x => new
            {
                x.actividaddetalle1.Id,
                x.cronograma1.nombremes,
                x.actividaddetalle1.IdConvocatoriaproyectoactividad,


            })
            .Where(x => x.IdConvocatoriaproyectoactividad == id)
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
        public async Task<IActionResult> OnPostNewCronogramaAsync(InvestigacionfomentoConvocatoriaproyectoactividaddetalleCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");


            var investigacionfomentoconvocatoriaproyectoactividaddetalle = new InvestigacionfomentoConvocatoriaproyectoactividaddetalle
            {
                IdConvocatoriaproyectocronograma = viewModel.IdConvocatoriaproyectocronograma,
                IdConvocatoriaproyectoactividad = viewModel.Id,
            };

            await _context.InvestigacionfomentoConvocatoriaproyectosactividadesdetalles.AddAsync(investigacionfomentoconvocatoriaproyectoactividaddetalle);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnPostDeletecronogramaAsync(Guid id)
        {
            var investigacionfomentoConvocatoriaproyectosactividadesdetalle = await _context.InvestigacionfomentoConvocatoriaproyectosactividadesdetalles.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoConvocatoriaproyectosactividadesdetalle == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoConvocatoriaproyectosactividadesdetalles.Remove(investigacionfomentoConvocatoriaproyectosactividadesdetalle);
            await _context.SaveChangesAsync();
            return new OkResult();
        }
        public async Task<IActionResult> OnGetDetailPlanAnexofaltanteAsync(Guid id)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriaproyecto, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.nombre;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }


            var queryrequisito = _context.InvestigacionfomentoRequisitos.AsNoTracking();
            var query = _context.InvestigacionfomentoConvocatoriaproyectosrequisitos.AsNoTracking();
            int recordsFiltered = await query.CountAsync();
            var data = await query
            .Join(queryrequisito,
                convocatoriaproyectorequisito1 => convocatoriaproyectorequisito1.IdRequisito,
                requisito1 => requisito1.Id,
                 (convocatoriaproyectorequisito1, requisito1) => new { convocatoriaproyectorequisito1, requisito1 })
            .Select(x => new
            {
                IdProyecto = x.convocatoriaproyectorequisito1.IdConvocatoriaproyecto,
                IdProyectorequisito = x.convocatoriaproyectorequisito1.Id,
                nombreRequisito = x.requisito1.nombre,
                idRequisito = x.requisito1.Id,
                archivourl = x.convocatoriaproyectorequisito1.archivourl,
                archivourlproyecto = x.convocatoriaproyectorequisito1.archivourlproyecto ?? "",

            })
            .Where(x => x.IdProyecto == id)
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
        public async Task<IActionResult> OnPostEnviarAnexofaltanteAsync(InvestigacionfomentoConvocatoriaproyectorequisitoCreateViewModel viewModel)
        {


            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");


            Console.WriteLine(333);

            var archivourl = "";
            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                archivourl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.CARTAPRESENTACION,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.INVESTIGACION_FOMENTO);
            }

            var InvestigacionfomentoConvocatoriaproyectorequisitos = await _context.InvestigacionfomentoConvocatoriaproyectosrequisitos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();
            InvestigacionfomentoConvocatoriaproyectorequisitos.archivourlproyecto = archivourl;
            await _context.SaveChangesAsync();



            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnGetDetailPlanActividadespresupuestoAsync(Guid id)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriaproyectoactividad, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.nombremes;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombremes;
                    break;
            }


            var querygastotipo = _context.InvestigacionfomentoGastotipos.AsNoTracking();
            var queryunidadmedida = _context.InvestigacionfomentoUnidadmedidas.AsNoTracking();


            var query = _context.InvestigacionfomentoConvocatoriaproyectopresupuestos.AsNoTracking();
            int recordsFiltered = await query.CountAsync();
            var data = await query
            .Join(querygastotipo,
              presupuesto1=>presupuesto1.IdGastotipo,
              gastotipo1=>gastotipo1.Id,
              (presupuesto1, gastotipo1) => new { presupuesto1, gastotipo1 })
            .Join(queryunidadmedida,
              presupuesto2=>presupuesto2.presupuesto1.IdUnidadmedida,
              unidadmedida1=>unidadmedida1.Id,
               (presupuesto2, unidadmedida1) => new { presupuesto2, unidadmedida1 })
            .Select(x => new
            {
                nombregastotipo =  x.presupuesto2.gastotipo1.nombre,
                nombreunidadmedida =  x.unidadmedida1.nombre,
                descripcion =  x.presupuesto2.presupuesto1.descripcion,
                costounitario =  x.presupuesto2.presupuesto1.costounitario,
                cantidad =  x.presupuesto2.presupuesto1.cantidad,
                total =  x.presupuesto2.presupuesto1.total,
                id =  x.presupuesto2.presupuesto1.Id,
                IdConvocatoriaproyecto = x.presupuesto2.presupuesto1.IdConvocatoriaproyecto,


            })
            .Where(x => x.IdConvocatoriaproyecto == id)
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
        public async Task<IActionResult> OnPostNewActividadpresupuestoAsync(InvestigacionfomentoConvocatoriaproyectopresupuestoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");


            var InvestigacionfomentoConvocatoriaproyectopresupuesto = new InvestigacionfomentoConvocatoriaproyectopresupuesto
            {
                descripcion = viewModel.descripcion,
                IdConvocatoriaproyecto = viewModel.IdConvocatoriaproyecto,
                IdConcepto=Guid.Parse("00000000-0000-0000-0000-000000000000"),
                IdUnidadmedida=viewModel.IdUnidadmedida,
                IdGastotipo=viewModel.IdGastotipo,
                costounitario=viewModel.costounitario,
                cantidad=viewModel.cantidad,
                total=viewModel.cantidad * viewModel.costounitario,
            };

            await _context.InvestigacionfomentoConvocatoriaproyectopresupuestos.AddAsync(InvestigacionfomentoConvocatoriaproyectopresupuesto);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnPostDeletecronogramapresupuestoAsync(Guid id)
        {
            var InvestigacionfomentoConvocatoriaproyectopresupuesto = await _context.InvestigacionfomentoConvocatoriaproyectopresupuestos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (InvestigacionfomentoConvocatoriaproyectopresupuesto == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoConvocatoriaproyectopresupuestos.Remove(InvestigacionfomentoConvocatoriaproyectopresupuesto);
            await _context.SaveChangesAsync();
            return new OkResult();
        }

    }
}
