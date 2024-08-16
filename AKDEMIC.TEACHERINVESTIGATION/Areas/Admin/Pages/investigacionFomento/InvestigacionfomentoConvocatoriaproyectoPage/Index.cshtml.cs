using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoViewModels;
using DocumentFormat.OpenXml.Wordprocessing;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoflujoindicadorViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationProjectViewModels;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Hosting;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoPdfCartapresentacionViewModels;
using AKDEMIC.CORE.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Text;
using DocumentFormat.OpenXml.Bibliography;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Microsoft.AspNetCore.Identity;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoViewModels;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;

using Microsoft.WindowsAzure.Storage.Auth;

using DocumentFormat.OpenXml.Office2010.Excel;
using System.Web;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoactividadViewModels;
using Microsoft.EntityFrameworkCore.Internal;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.MaestroAlumnoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.MaestroAreaViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectorequisitoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividadViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoactividaddetalleViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectomiembroViewModels;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoPage
{
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
         GeneralConstants.ROLES.TEACHERS + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        private readonly IConverter _dinkConverter;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        protected readonly AkdemicContext _context;
        private readonly IDataTablesService _dataTablesService;
        private readonly IOptions<CloudStorageCredentials> _storageCredentials;
        private readonly UserManager<ApplicationUser> _userManager;



        public IndexModel(
              IConverter dinkConverter,
            IWebHostEnvironment hostingEnvironment,
            IViewRenderService viewRenderService,
            AkdemicContext context,
            IDataTablesService dataTablesService,
            IOptions<CloudStorageCredentials> storageCredentials,
                        UserManager<ApplicationUser> userManager


        )
        {
            _dinkConverter = dinkConverter;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _context = context;
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;
            _userManager = userManager;


        }
        public InvestigacionfomentoConvocatoriaproyectoEditViewModel input { get; set; }
        Guid xIdOficina = Guid.Parse("e0557aa7-6404-11ee-b7b1-16d13ee00159");
        public async Task OnGetRevisionAsync()

        {
            string tipousuario = "R";
            // string nombretipoplan = "Docente";


            //ViewData["tieneplan"] = "0";
            var user = await _userManager.GetUserAsync(User);

            var queryMaestroAreas = _context.MaestroAreas
                .AsNoTracking();


            var data =  _context.MaestroAreasusuarios
                .Join(queryMaestroAreas,
                areausuario1 => areausuario1.IdArea,
                area1 => area1.Id,
                (areausuario1, area1) => new { areausuario1, area1 })
                .Select(
                    x => new
                    {
                        Id = x.area1.Id,
                        nombre = x.area1.nombre,
                        IdUser = x.areausuario1.IdUser,
                    }
                )
                .Where(x=>x.IdUser==Guid.Parse(user.Id))
                .ToList();
           

           


           

            ViewData["tipousuario"] = tipousuario;
            // ViewData["nombretipoplan"] = nombretipoplan;

        }

        public InvestigacionfomentoConvocatoriaproyectoCreateViewModel fechaini { get; set; }
        public InvestigacionfomentoConvocatoriaproyectoCreateViewModel fechafin { get; set; }

        public async Task<IActionResult> OnPostCreateAsync(InvestigacionfomentoConvocatoriaproyectoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");
            var user = await _userManager.GetUserAsync(User);

            var maestrodocente = await _context.MaestroDocentes.Where(x => x.IdUser == Guid.Parse( user.Id)).FirstOrDefaultAsync();


            var investigacionfomentoconvocatoria = new InvestigacionfomentoConvocatoriaproyecto
            {
                IdOficina = xIdOficina,
                nombre = viewModel.nombre,
                IdConvocatoria = viewModel.IdConvocatoria,
                IdLinea = viewModel.IdLinea,
                objetivoprincipal = viewModel.objetivoprincipal,
                presupuesto = viewModel.presupuesto,
                IdArea = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                estado = "0",
                IdDocente = maestrodocente.Id,
                nromeses = viewModel.nromeses,
                retornadocente = "0",
        };

            await _context.InvestigacionfomentoConvocatoriaproyectos.AddAsync(investigacionfomentoconvocatoria);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionfomentoConvocatoriaproyectoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionfomentoconvocatoriaproyecto = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyecto == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoconvocatoriaproyecto.IdConvocatoria = viewModel.IdConvocatoria;
            investigacionfomentoconvocatoriaproyecto.IdLinea = viewModel.IdLinea;
            investigacionfomentoconvocatoriaproyecto.nombre = viewModel.nombre;
            investigacionfomentoconvocatoriaproyecto.objetivoprincipal = viewModel.objetivoprincipal;
            investigacionfomentoconvocatoriaproyecto.presupuesto = viewModel.presupuesto;
            investigacionfomentoconvocatoriaproyecto.nromeses = viewModel.nromeses;
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionfomentoconvocatoriaproyecto = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyecto == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionfomentoConvocatoriaproyectos.Remove(investigacionfomentoconvocatoriaproyecto);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionfomentoconvocatoriaproyecto = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyecto == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentoconvocatoriaproyecto.Id,
                investigacionfomentoconvocatoriaproyecto.nombre,
                investigacionfomentoconvocatoriaproyecto.presupuesto,
                investigacionfomentoconvocatoriaproyecto.IdConvocatoria,
                investigacionfomentoconvocatoriaproyecto.IdLinea,
                investigacionfomentoconvocatoriaproyecto.objetivoprincipal,
                investigacionfomentoconvocatoriaproyecto.estado ,
                investigacionfomentoconvocatoriaproyecto.nromeses,

            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, string tipousuario = "")
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriaproyecto, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nombre;
                    break;
            }

            var queryconvocatoria = _context.InvestigacionfomentoConvocatorias
                 .Where(x => x.IdOficina == xIdOficina)
                 .AsNoTracking();
            var querylinea = _context.MaestroLineas.AsNoTracking();

            var query = _context.InvestigacionfomentoConvocatoriaproyectos
                .Where(x => x.IdOficina == xIdOficina)
                .AsNoTracking();


            int recordsFiltered = await query.CountAsync();
           
            if (tipousuario == "R")
            {
                if (string.IsNullOrEmpty(searchValue))
                {
                    searchValue= "00000000-0000-0000-0000-000000000000";

                }

                    var queryproyectodetalle =  _context.InvestigacionfomentoConvocatoriaproyectosflujos
                    .Where(x=>x.IdArea == Guid.Parse(searchValue) && x.estado!="1")
                    .AsNoTracking();

                var queryproyectodetalle2 =  _context.InvestigacionfomentoConvocatoriaproyectosflujos
                    .Join(queryproyectodetalle,
                    queryproyectodetalle2_1=> queryproyectodetalle2_1.IdConvocatoriaproyecto,
                    queryproyectodetalle_1=> queryproyectodetalle_1.IdConvocatoriaproyecto,
                    (queryproyectodetalle2_1, queryproyectodetalle_1) => new { queryproyectodetalle2_1, queryproyectodetalle_1 })
                   .Select(x => new
                   {
                       IdConvocatoriaproyecto = x.queryproyectodetalle_1.IdConvocatoriaproyecto,
                       Id= x.queryproyectodetalle_1.Id,
                       IdArea =x.queryproyectodetalle_1.IdArea,
                       retornadocente = x.queryproyectodetalle_1.retornadocente ?? "0",
                       orden1= x.queryproyectodetalle_1.orden,
                       orden2 = x.queryproyectodetalle2_1.orden,
                       estado2= x.queryproyectodetalle2_1.estado,
                   })
                   .Where(x => (x.orden1 == 1 ? x.orden2 == 1 : x.orden2 == (x.orden1 - 1) && x.estado2.ToLower().Trim().Contains("1")))
                   .AsNoTracking();


                var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(queryconvocatoria,
                    convocatoriaproyecto1 => convocatoriaproyecto1.IdConvocatoria,
                    convocatoria1 => convocatoria1.Id,
                     (convocatoriaproyecto1, convocatoria1) => new { convocatoriaproyecto1, convocatoria1 })
                .Join(querylinea,
                    convocatoriaproyecto2 => convocatoriaproyecto2.convocatoriaproyecto1.IdLinea,
                    linea1 => linea1.Id,
                     (convocatoriaproyecto2, linea1) => new { convocatoriaproyecto2, linea1 })
                .Join(queryproyectodetalle2,
                convocatoriaproyecto3=> convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.Id,
                queryproyectodetalle2_1=> queryproyectodetalle2_1.IdConvocatoriaproyecto,
                (convocatoriaproyecto3 , queryproyectodetalle2_1) => new { convocatoriaproyecto3 , queryproyectodetalle2_1 })
                .Select(x => new
                {
                    Id = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.Id,
                    presupuesto = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.presupuesto,
                    nombre = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.nombre,
                    nombreconvocatoria = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoria1.nombre,
                    nombrelinea = x.convocatoriaproyecto3.linea1.titulo,
                    estado = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.estado,
                    archivourlcarta = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.archivourlcarta ?? "",
                    tipousuario = tipousuario,
                    idArea = x.queryproyectodetalle2_1.IdArea,
                    retornadocente = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.retornadocente, // x.queryproyectodetalle2_1.retornadocente,
                    idProyectoflujo = x.queryproyectodetalle2_1.Id,
                    IdOficina = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoria1.IdOficina,
                    retornadocenteproyecto = x.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.retornadocente,
                })
              .Where(x => (tipousuario == "R" ? x.estado == "1" : "1" == "1") && x.retornadocente == "0")
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
            else
            {
                var user = await _userManager.GetUserAsync(User);

                var maestrodocente = await _context.MaestroDocentes.Where(x => x.IdUser == Guid.Parse(user.Id)).FirstOrDefaultAsync();


                var data = await query
                                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                                .Join(queryconvocatoria,
                                    convocatoriaproyecto1 => convocatoriaproyecto1.IdConvocatoria,
                                    convocatoria1 => convocatoria1.Id,
                                     (convocatoriaproyecto1, convocatoria1) => new { convocatoriaproyecto1, convocatoria1 })
                                .Join(querylinea,
                                    convocatoriaproyecto2 => convocatoriaproyecto2.convocatoriaproyecto1.IdLinea,
                                    linea1 => linea1.Id,
                                     (convocatoriaproyecto2, linea1) => new { convocatoriaproyecto2, linea1 })
                                .Select(x => new
                                {
                                    Id = x.convocatoriaproyecto2.convocatoriaproyecto1.Id,
                                    presupuesto = x.convocatoriaproyecto2.convocatoriaproyecto1.presupuesto,
                                    nombre = x.convocatoriaproyecto2.convocatoriaproyecto1.nombre,
                                    nombreconvocatoria = x.convocatoriaproyecto2.convocatoria1.nombre,
                                    nombrelinea = x.linea1.titulo,
                                    estado = x.convocatoriaproyecto2.convocatoriaproyecto1.estado,
                                    archivourlcarta = x.convocatoriaproyecto2.convocatoriaproyecto1.archivourlcarta ?? "",
                                    tipousuario = tipousuario,
                                    retornadocente = x.convocatoriaproyecto2.convocatoriaproyecto1.retornadocente,
                                    x.convocatoriaproyecto2.convocatoriaproyecto1.IdDocente,
                                    IdOficina=x.convocatoriaproyecto2.convocatoriaproyecto1.IdOficina,
                                })
                                .Where(x => (tipousuario == "R" ? x.estado == "1" : "1" == "1" && x.IdDocente == maestrodocente.Id) && x.IdOficina == xIdOficina)
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
        public async Task<IActionResult> OnPostEnviarProyectoAsync(InvestigacionfomentoConvocatoriaproyectoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionfomentoconvocatoriaproyectos = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyectos == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoconvocatoriaproyectos.estado = "1";
            Guid idconvocatoria = investigacionfomentoconvocatoriaproyectos.IdConvocatoria;
            await _context.SaveChangesAsync();

            var investigacionfomentoconvocatoriaproyectoshistoriales = new InvestigacionfomentoConvocatoriaproyectohistorial
            {
                IdConvocatoriaproyecto = viewModel.Id,
                observacion = "Enviado para revisión",
                estado = "1",
            };

            await _context.InvestigacionfomentoConvocatoriaproyectoshistoriales.AddAsync(investigacionfomentoconvocatoriaproyectoshistoriales);
            await _context.SaveChangesAsync();

            var queryflujoarea = _context.InvestigacionfomentoFlujosareas.AsNoTracking();
            var queryconvenio = _context.InvestigacionfomentoConvocatorias
            .AsNoTracking()
              .Join(queryflujoarea,
                  convocatoria1 => convocatoria1.IdFlujo,
                  flujoarea1 => flujoarea1.IdFlujo,
                   (convocatoria1, flujoarea1) => new { convocatoria1, flujoarea1 })
              .Where(x => x.convocatoria1.Id == idconvocatoria)
              .Select(x => new
              {
                  IdFlujo = x.convocatoria1.IdFlujo,
                  IdArea = x.flujoarea1.IdArea,
                  orden = x.flujoarea1.orden,
                  retornadocente = x.flujoarea1.retornadocente,

              }).ToList();

            foreach(var item  in queryconvenio)
            {
                Console.WriteLine(item.IdArea);
                var investigacionfomentoconvocatoriaproyectosflujos = new InvestigacionfomentoConvocatoriaproyectoflujo
                {
                    IdConvocatoriaproyecto = viewModel.Id,
                    IdArea = item.IdArea,
                    IdFlujo = item.IdFlujo,
                    orden = item.orden,
                    retornadocente = item.retornadocente,
                    estado = "0",
                };
                await _context.InvestigacionfomentoConvocatoriaproyectosflujos.AddAsync(investigacionfomentoconvocatoriaproyectosflujos);
                await _context.SaveChangesAsync();
            }
            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnPostPdfcartapresentacionAsync(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);
            
            var querylinea = _context.MaestroLineas.AsNoTracking();

            var investigacionfomentoconvocatoriaproyecto = await _context
                .InvestigacionfomentoConvocatoriaproyectos
                .Join(querylinea,
                 convocatoria1=>convocatoria1.IdLinea,
                 linea1=>linea1.Id,
                 (convocatoria1, linea1) => new { convocatoria1 , linea1 }  )
                .Where(x => x.convocatoria1.Id == id)
                .Select(x => new
                {  presupuesto = x.convocatoria1.presupuesto,
                   objetivoprincipal = x.convocatoria1.objetivoprincipal,
                   nombre = x.convocatoria1.nombre,
                   nombrelinea = x.linea1.titulo,

                })
                .FirstOrDefaultAsync();
            var modal = new InvestigacionfomentoPdfCartapresentacionRptViewModel
            {
                nombrelinea= investigacionfomentoconvocatoriaproyecto.nombrelinea,
                objetivoprincipal = investigacionfomentoconvocatoriaproyecto.objetivoprincipal,
                nombreproyecto = investigacionfomentoconvocatoriaproyecto.nombre,
                presupuesto = investigacionfomentoconvocatoriaproyecto.presupuesto.ToString(),
                FullName = user.FullName,
            };

            Console.WriteLine(FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);


            modal.ImageLogoPath = Path.Combine(_hostingEnvironment.WebRootPath, $@"images\themes\{GeneralConstants.GetTheme()}\logo-report.png");

            var viewToString = await _viewRenderService.RenderToStringAsync("/Areas/Admin/Pages/investigacionFomento/InvestigacionfomentoConvocatoriaproyectoPage/Partials/_CartapresentacionReportPdfcshtml.cshtml", modal);
            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8" },
                FooterSettings = {
                        FontName = "Arial",
                        FontSize = 9,
                        Line = false,
                        Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                        Center = "",
                        Right = "Pag: [page]/[toPage]"
                }
            };
            var directorio = Directory.GetCurrentDirectory();
            var direc = "/usr/share/nginx/html/common/sigau/2023/cartapresentacion/cartapresentacion"+id + ".pdf";
            var direc2 = "/documentos/2023/cartapresentacion/cartapresentacion" + id + ".pdf";
            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 30, Bottom = 10, Left = 30, Right = 30 },
                DocumentTitle = "prueba",
                Out = @direc
                //Out = @""+ directorio+ "/cartapresentacion.pdf",
            };
            var pdf = new DinkToPdf.HtmlToPdfDocument
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);
            var file = File(fileByte, "application/pdf", "cartapresentacion.pdf"); 
            var result = new
            {
                file = direc2
            };
            return new OkObjectResult(result);

            

        }
        public async Task<IActionResult> OnGetDetailFileAsync(Guid id)
        {
            var investigacionfomentoconvocatoriaproyectos = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyectos == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionfomentoconvocatoriaproyectos.Id,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEditFileAsync(InvestigacionfomentoConvocatoriaproyectoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionfomentoconvocatoriaproyectos = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyectos == null)
                return new BadRequestObjectResult("Sucedio un error");



            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigacionfomentoconvocatoriaproyectos.archivourlcarta = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.CARTAPRESENTACION,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.INVESTIGACION_FOMENTO);
            }

            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnGetDetailPlanObservacionAsync(Guid id, Guid idArea)
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
            var querylistaverificacion = _context.InvestigacionfomentoListaverificaciones.AsNoTracking();
            var querylistaverificacionindicador = _context.InvestigacionfomentoListaverificacionindicadores.AsNoTracking();
            var queryindicadores = _context.InvestigacionfomentoIndicadores.AsNoTracking();
            var queryconvocatoria = _context.InvestigacionfomentoConvocatorias.AsNoTracking();
            var queryconvocatorialistaverificaciones = _context.InvestigacionfomentoConvocatorialistaverificaciones.AsNoTracking();

            var queryflujo= _context.InvestigacionfomentoFlujos.AsNoTracking();
            var queryflujoarea = _context.InvestigacionfomentoFlujosareas.AsNoTracking();

            var queryconvocatoriaproyectosflujo = _context.InvestigacionfomentoConvocatoriaproyectosflujos
                .Where(x=>x.IdArea== idArea)
                .AsNoTracking();

            var query = _context.InvestigacionfomentoConvocatoriaproyectos
                .AsNoTracking();


            int recordsFiltered = await query.CountAsync();



          

            var data = await query
            .Join(queryconvocatorialistaverificaciones,
                convocatoriaproyecto1 => convocatoriaproyecto1.IdConvocatoria,
                convocatorialistaverificaciones1 => convocatorialistaverificaciones1.IdConvocatoria,
                 (convocatoriaproyecto1, convocatorialistaverificaciones1) => new { convocatoriaproyecto1, convocatorialistaverificaciones1 })
            .Join(querylistaverificacionindicador,
                convocatoriaproyecto2 => convocatoriaproyecto2.convocatorialistaverificaciones1.IdListaverificacion,
                listaverificacionindicador1 => listaverificacionindicador1.IdListaverificacion,
                 (convocatoriaproyecto2, listaverificacionindicador1) => new { convocatoriaproyecto2, listaverificacionindicador1 })
            .Join(queryindicadores,
            convocatoriaproyecto3 => convocatoriaproyecto3.listaverificacionindicador1.IdIndicador,
            indicadores1 => indicadores1.Id,
            (convocatoriaproyecto3, indicadores1) => new { convocatoriaproyecto3, indicadores1 })
             .Join(queryconvocatoriaproyectosflujo,
            convocatoriaproyecto4 => convocatoriaproyecto4.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.Id,
            convocatoriaproyectosflujo1 => convocatoriaproyectosflujo1.IdConvocatoriaproyecto,
            (convocatoriaproyecto4, convocatoriaproyectosflujo1) => new { convocatoriaproyecto4, convocatoriaproyectosflujo1 })             
            .Select(x => new
            {
                IdIndicador = x.convocatoriaproyecto4.indicadores1.Id,
                nombreindicador = x.convocatoriaproyecto4.indicadores1.nombre,
                si = "0",
                no = "0",
                IdProyecto = x.convocatoriaproyecto4.convocatoriaproyecto3.convocatoriaproyecto2.convocatoriaproyecto1.Id,
                IdArea = x.convocatoriaproyecto4.convocatoriaproyecto3.convocatoriaproyecto2.convocatorialistaverificaciones1.IdArea,
                IdListaverificacion = x.convocatoriaproyecto4.convocatoriaproyecto3.listaverificacionindicador1.Id,
                retornadocente = x.convocatoriaproyectosflujo1.retornadocente,
                IdProyectoflujo = x.convocatoriaproyectosflujo1.Id

                
            })
            .Where(x => x.IdProyecto == id && x.IdArea == idArea)
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
        public async Task<IActionResult> OnPostEnviarRevisionproyectoAsync(InvestigacionfomentoConvocatoriaproyectoflujoindicadorCreateViewModel viewModel)
        {
            
         
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            Guid id = viewModel.Id;

            if (viewModel.valores!=null)
            {
               

            

            int n = viewModel.valores.Length;
            for (int i = 0; i < n; i++)
            {
                
                var Investigacionfomentoconvocatoriaproyectoflujoindicador = new InvestigacionfomentoConvocatoriaproyectoflujoindicador
                {
                    IdConvocatoriaproyecto = viewModel.IdConvocatoriaproyectos[i],
                    IdListaverificacion = viewModel.IdListaverificaciones[i],
                    IdArea = viewModel.IdArea,
                    IdIndicador = viewModel.IdIndicadores[i],
                    valor = viewModel.valores[i],
                    puntaje = viewModel.puntajes[i],
                    observacion = viewModel.observaciones[i],
                };
                await _context.investigacionfomentoConvocatoriasproyectosflujosindicadores.AddAsync(Investigacionfomentoconvocatoriaproyectoflujoindicador);
                await _context.SaveChangesAsync();
            }
            }
           
            var investigacionfomentoconvocatoriaproyectos = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyectos == null)
                return new BadRequestObjectResult("Sucedio un error");
            if (viewModel.estado == "0")
            {
                investigacionfomentoconvocatoriaproyectos.estado = "3";
            }
            else
            {
                investigacionfomentoconvocatoriaproyectos.estado = "1";

            }
            investigacionfomentoconvocatoriaproyectos.IdArea = viewModel.IdArea;
            investigacionfomentoconvocatoriaproyectos.retornadocente = viewModel.Retornadocente;
            await _context.SaveChangesAsync();
           
            var investigacionfomentoconvocatoriaproyectoshistoriales = new InvestigacionfomentoConvocatoriaproyectohistorial
            {
                IdConvocatoriaproyecto = id,
                observacion =viewModel.observacion,
                estado = viewModel.estado,
            };

            await _context.InvestigacionfomentoConvocatoriaproyectoshistoriales.AddAsync(investigacionfomentoconvocatoriaproyectoshistoriales);
            await _context.SaveChangesAsync();
          
            var InvestigacionfomentoConvocatoriaproyectoflujos = await _context.InvestigacionfomentoConvocatoriaproyectosflujos.Where(x => x.Id == viewModel.IdProyectoflujo).FirstOrDefaultAsync();
            InvestigacionfomentoConvocatoriaproyectoflujos.estado = (viewModel.estado == "0" ? "2" : "1");
            InvestigacionfomentoConvocatoriaproyectoflujos.observacion = viewModel.observacion;
            await _context.SaveChangesAsync();
          

            return new OkObjectResult("");
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
                idRequisito =  x.requisito1.Id,
                archivourl =  x.convocatoriaproyectorequisito1.archivourl,
                archivourlproyecto = x.convocatoriaproyectorequisito1.archivourlproyecto??"",

            })
            .Where(x=>x.IdProyecto == id)            
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
        public async Task<IActionResult> OnPostEnviarAnexoproyectoAsync(InvestigacionfomentoConvocatoriaproyectoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionfomentoconvocatoriaproyectos = await _context.InvestigacionfomentoConvocatoriaproyectos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyectos == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigacionfomentoconvocatoriaproyectos.retornadocente = "0";
            await _context.SaveChangesAsync();

            var investigacionfomentoconvocatoriaproyectoshistoriales = new InvestigacionfomentoConvocatoriaproyectohistorial
            {
                IdConvocatoriaproyecto = viewModel.Id,
                observacion = "Enviado anexos revisión",
                estado = "1",
            };

            await _context.InvestigacionfomentoConvocatoriaproyectoshistoriales.AddAsync(investigacionfomentoconvocatoriaproyectoshistoriales);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
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
                estado="0"
            };

            await _context.InvestigacionfomentoConvocatoriaproyectosactividades.AddAsync(investigacionfomentoconvocatoriaproyectoactividad);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
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
        public async Task<IActionResult> OnPostEnviarActividadAsync(InvestigacionfomentoConvocatoriaproyectoactividadEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionfomentoconvocatoriaproyectosactividad = await _context.InvestigacionfomentoConvocatoriaproyectosactividades.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionfomentoconvocatoriaproyectosactividad == null)
                return new BadRequestObjectResult("Sucedio un error");

            var archivourl = "";
            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                archivourl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PROYECTOACTIVIDAD,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.INVESTIGACION_FOMENTO);
            }

            investigacionfomentoconvocatoriaproyectosactividad.estado = viewModel.estado;
            investigacionfomentoconvocatoriaproyectosactividad.titulo = viewModel.titulo;
            investigacionfomentoconvocatoriaproyectosactividad.archivourl = archivourl;
            await _context.SaveChangesAsync();

           /* var investigacionfomentoconvocatoriaproyectoshistoriales = new InvestigacionfomentoConvocatoriaproyectohistorial
            {
                IdConvocatoriaproyecto = viewModel.Id,
                observacion = "Enviado anexos revisión",
                estado = "1",
            };

            await _context.InvestigacionfomentoConvocatoriaproyectoshistoriales.AddAsync(investigacionfomentoconvocatoriaproyectoshistoriales);
            await _context.SaveChangesAsync();*/

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
            actividaddetalle1=>actividaddetalle1.IdConvocatoriaproyectocronograma,
            cronograma1=> cronograma1.Id,
            (actividaddetalle1, cronograma1) =>new { actividaddetalle1, cronograma1 })
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

        public async Task<IActionResult> OnGetDetailPlanMiembrosAsync(Guid id)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionfomentoConvocatoriaproyectomiembro, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.apellidopaterno;
                    break;
                case "1":
                    orderByPredicate = (x) => x.apellidomaterno;
                    break;
            }


  


            var query = _context.InvestigacionfomentoConvocatoriaproyectosmiembros.AsNoTracking();
            int recordsFiltered = await query.CountAsync();
            var data = await query
            .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)            
            .Select(x => new
            {
               x.Id,
               x.IdConvocatoriaproyecto,
               x.apellidopaterno,
               x.apellidomaterno,
               x.nombres,
               x.numerodocumento,

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
        public async Task<IActionResult> OnPostEnviarMiembroAsync(InvestigacionfomentoConvocatoriaproyectomiembroCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

           
            var investigacionfomentoconvocatoriaproyectomiembro = new InvestigacionfomentoConvocatoriaproyectomiembro
            {
                IdConvocatoriaproyecto =viewModel.IdConvocatoriaproyecto,
               apellidopaterno = viewModel.apellidopaterno,
               apellidomaterno = viewModel.apellidomaterno,
                nombres = viewModel.nombres,
                numerodocumento = viewModel.numerodocumento,
            };

            await _context.InvestigacionfomentoConvocatoriaproyectosmiembros.AddAsync(investigacionfomentoconvocatoriaproyectomiembro);
            await _context.SaveChangesAsync();
            return new OkObjectResult("");
        }

    }

}
