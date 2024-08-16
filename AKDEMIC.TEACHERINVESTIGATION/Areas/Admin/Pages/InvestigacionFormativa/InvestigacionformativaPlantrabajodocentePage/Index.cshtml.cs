using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Options;
using System.IO;

using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels;
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
using DocumentFormat.OpenXml.Spreadsheet;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.InvestigacionformativaPlantrabajodocentePage
{
    [Authorize(Roles = GeneralConstants.ROLES.ACOMPANIANTE + "," + 
        GeneralConstants.ROLES.EVALUATOR_COMMITTEE + "," + 
        GeneralConstants.ROLES.TEACHERS + "," +
        GeneralConstants.ROLES.SUPERADMIN + "," +
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
        public async Task OnGetTipoPlanDocenteAsync()
        {
            string tipoplan = "D";
            string nombretipoplan = "Docente";
            ViewData["idanio"] = "00000000-0000-0000-0000-000000000000";
            ViewData["anio"] = "";
            var investigacionformativaconfiguraranios = await _context.InvestigacionformativaConfiguracionanios.Where(x => x.activo=="1").FirstOrDefaultAsync();
            if (investigacionformativaconfiguraranios != null) {
                String anio = investigacionformativaconfiguraranios.nombre;
                Guid idanio = investigacionformativaconfiguraranios.Id;
                ViewData["idanio"] = idanio;
                ViewData["anio"] = anio;

            }
           
            ViewData["tieneplan"] = "0";
            var user = await _userManager.GetUserAsync(User);
            String iddocente = "";
          
            var maestrodocente = await _context.MaestroDocentes.Where(x => x.IdUser == Guid.Parse(user.Id)).FirstOrDefaultAsync();

            if (maestrodocente != null)
            {

                iddocente = maestrodocente.Id.ToString();
            }
            //Console.WriteLine(iddocente);
            //Console.WriteLine(Guid.Parse(ViewData["idanio"].ToString()));
            var investigacionformativaplantrabajo = await _context.InvestigacionformativaPlantrabajos.Where(x => x.IdAnio == Guid.Parse(ViewData["idanio"].ToString()) && x.IdDocente == Guid.Parse(iddocente) ).FirstOrDefaultAsync();
            if (investigacionformativaplantrabajo != null)
            {
                ViewData["tieneplan"] = "1";
            }

            ViewData["tipoplan"] = tipoplan;
            ViewData["nombretipoplan"] = nombretipoplan;
           

        }
        public void OnGetTipoPlanComite()
        {
            string tipoplan = "C";
            string nombretipoplan = "Acompañante";

            ViewData["tipoplan"] = tipoplan;
            ViewData["nombretipoplan"] = nombretipoplan;

        }
        public void OnGetTipoPlanComiteFinal()
        {
            string tipoplan = "F";
            string nombretipoplan = "Comite Evaluador";

            ViewData["tipoplan"] = tipoplan;
            ViewData["nombretipoplan"] = nombretipoplan;

        }

        public InvestigacionformativaPlantrabajoCreateViewModel fechaini { get; set; }
        public InvestigacionformativaPlantrabajoCreateViewModel fechafin { get; set; }
        public async Task<IActionResult> OnPostCreateAsync(InvestigacionformativaPlantrabajoCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");


            /* string fileUrl = await storage.UploadFile(viewModel.File.OpenReadStream(),
                FileStorageConstants.CONTAINER_NAMES.PLANTRABAJO,
                    Path.GetExtension(viewModel.File.FileName),
                    FileStorageConstants.SystemFolder.INVESTIGACION_FORMATIVA);*/

            var investigacionformativaplantrabajos = new InvestigacionformativaPlantrabajo
            {
                IdDocente = viewModel.IdDocente,
                IdDepartamento = viewModel.IdDepartamento,
                IdAreaacademica = viewModel.IdAreaacademica,
                IdCarrera = viewModel.IdCarrera,
                titulo = viewModel.titulo,
                objetivo = viewModel.objetivo,
                descripcion = viewModel.descripcion,
                IdLinea = viewModel.IdLinea,
                fechafin = viewModel.fechafin,
                fechaini = viewModel.fechaini,
                IdTiporesultado = viewModel.IdTiporesultado,
                IdTipoevento = viewModel.IdTipoevento,
                IdAnio = viewModel.IdAnio,
                estado = "0",
                archivourl = "",
                activo = viewModel.activo ?? "0",
            };

            await _context.InvestigacionformativaPlantrabajos.AddAsync(investigacionformativaplantrabajos);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionformativaPlantrabajoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null) return new BadRequestObjectResult("Sucedio un error");


            investigacionformativaplantrabajos.IdDepartamento = viewModel.IdDepartamento;
            investigacionformativaplantrabajos.IdDocente = viewModel.IdDocente;
            investigacionformativaplantrabajos.IdAreaacademica = viewModel.IdAreaacademica;
            investigacionformativaplantrabajos.titulo = viewModel.titulo;
            investigacionformativaplantrabajos.IdLinea = viewModel.IdLinea;
            investigacionformativaplantrabajos.objetivo = viewModel.objetivo;
            investigacionformativaplantrabajos.descripcion = viewModel.descripcion;
            investigacionformativaplantrabajos.fechaini = viewModel.fechaini;
            investigacionformativaplantrabajos.fechafin = viewModel.fechafin;
            investigacionformativaplantrabajos.descripcion = viewModel.descripcion;
            investigacionformativaplantrabajos.IdCarrera = viewModel.IdCarrera;
            investigacionformativaplantrabajos.IdTipoevento = viewModel.IdTipoevento;
            investigacionformativaplantrabajos.IdTiporesultado = viewModel.IdTiporesultado;
            investigacionformativaplantrabajos.activo = viewModel.activo ?? "0";
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionformativaPlantrabajos.Remove(investigacionformativaplantrabajos);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null) return new BadRequestObjectResult("Sucedio un error");



            var result = new
            {
                investigacionformativaplantrabajos.Id,
                investigacionformativaplantrabajos.IdDocente,
                investigacionformativaplantrabajos.IdDepartamento,
                investigacionformativaplantrabajos.IdCarrera,
                investigacionformativaplantrabajos.titulo,
                investigacionformativaplantrabajos.IdAreaacademica,
                investigacionformativaplantrabajos.objetivo,
                investigacionformativaplantrabajos.descripcion,
                investigacionformativaplantrabajos.IdTiporesultado,
                investigacionformativaplantrabajos.IdTipoevento,
                investigacionformativaplantrabajos.fechaini,
                investigacionformativaplantrabajos.fechafin,
                investigacionformativaplantrabajos.IdLinea,
                investigacionformativaplantrabajos.archivourl,
                investigacionformativaplantrabajos.activo,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnGetDetailFileAsync(Guid id)
        {
            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionformativaplantrabajos.Id,
                investigacionformativaplantrabajos.IdTiporesultado,
                investigacionformativaplantrabajos.IdTipoevento,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditFileAsync(InvestigacionformativaPlantrabajoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null)
                return new BadRequestObjectResult("Sucedio un error");



            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigacionformativaplantrabajos.archivourl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnGetDetailUsuariosAsync(Guid id)
        {
            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionformativaplantrabajos.Id,
                investigacionformativaplantrabajos.IdTiporesultado,
                investigacionformativaplantrabajos.IdTipoevento,
                investigacionformativaplantrabajos.coautor1,
                investigacionformativaplantrabajos.coautor2,
            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditAutoresAsync(InvestigacionformativaPlantrabajoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigacionformativaplantrabajos.coautor1 = viewModel.coautor1;
            investigacionformativaplantrabajos.coautor2 = viewModel.coautor2;

            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnPostEnviarPlanAsync(InvestigacionformativaPlantrabajoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigacionformativaplantrabajos.estado = "1";
            await _context.SaveChangesAsync();

            var investigacionformativaplantrabajoshistoriales = new InvestigacionformativaPlantrabajohistorial
            {
                IdPlantrabajo = viewModel.Id,
                observacion = "Enviado para revisión",
                estado = "1",
            };

            await _context.InvestigacionformativaPlantrabajoshistoriales.AddAsync(investigacionformativaplantrabajoshistoriales);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, string tipoplan ="", string searchanioValue = null)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionformativaPlantrabajo, dynamic>> orderByPredicate = null;

           
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.titulo;
                    break;
            }

            var user = await _userManager.GetUserAsync(User);

            Guid Idfacultad = Guid.Parse("00000000-0000-0000-0000-000000000000");
            var querycomite = await _context.InvestigacionformativaComites.Where(x => x.IdUser == Guid.Parse(user.Id)).FirstOrDefaultAsync();
            if (querycomite != null)
            {
                Idfacultad = querycomite.IdFacultad;

            };

            var queryareaacademicas = _context.MaestroAreasacademicas.AsNoTracking();
            var queryfacultades = _context.MaestroFacultades.AsNoTracking();
            var querydepartamentos = _context.MaestroDepartamentos
                .AsNoTracking()
                .Join(queryfacultades,
                departamentos1 => departamentos1.IdFacultad,
                facultades1 => facultades1.Id,
                (departamentos1, facultades1) => new { departamentos1, facultades1 })
                .Select(
                    x => new
                    {
                        Id = x.departamentos1.Id,
                        nombre = x.departamentos1.nombre,
                        nombrefacultad = x.facultades1.nombre,
                        IdFacultad = x.facultades1.Id,
                    }
                );
            var admin = "0";
            if (tipoplan == "F")
            {
                if (Guid.Parse(user.Id) != Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4")) { 
                querydepartamentos = querydepartamentos.Where(x => x.IdFacultad == Idfacultad);
                    admin = "0";
                }
                else
                {
                    admin = "1";
                }
            }
            if (Guid.Parse(user.Id) == Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4"))
            {
                admin = "1";

            }
            var querylineas = _context.MaestroLineas.AsNoTracking();
            var querydocentes = _context.MaestroDocentes.AsNoTracking();
            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();

           

            var queryplantrabajoactividades = _context.InvestigacionformativaPlantrabajosactividades.AsNoTracking()
                .Where(x=>(tipoplan=="C"? x.informefinal == "0" : (tipoplan == "F" ? x.informefinal == "1" :1==1)))
                .GroupBy(g => new { g.IdPlantrabajo })
                .Select(x => new
                    {   
                        IdPlantrabajo = x.Key.IdPlantrabajo,
                        total = x.ToList().Count(),
                        totalenviado =  x.Sum( x=> x.estado=="1" ? 1:0  ),
                        totalaprobado = x.Sum(x => x.estado == "2" ? 1 : 0),
                        totalobservado = x.Sum(x => x.estado == "3" ? 1 : 0),
                    }
                );


            var query = _context.InvestigacionformativaPlantrabajos
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
               .Join(queryareaacademicas,
                   plantrabajos1 => plantrabajos1.IdAreaacademica,
                   areaacademica1 => areaacademica1.Id,
                    (plantrabajos1, areaacademica1) => new { plantrabajos1, areaacademica1 })
               .Join(querydepartamentos,
                   plantrabajos2 => plantrabajos2.plantrabajos1.IdDepartamento,
                   departamento1 => departamento1.Id,
                   (plantrabajos2, departamento1) => new { plantrabajos2, departamento1 })
                .Join(querylineas,
                   plantrabajos3 => plantrabajos3.plantrabajos2.plantrabajos1.IdLinea,
                   lineas1 => lineas1.Id,
                   (plantrabajos3, lineas1) => new { plantrabajos3, lineas1 })
               .Join(querydocentes,
                   plantrabajos4 => plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.IdDocente,
                   docentes1 => docentes1.Id,
                   (plantrabajos4, docentes1) => new { plantrabajos4, docentes1 })
               .Join(queryusuarios,
                   plantrabajos5 => plantrabajos5.docentes1.IdUser,
                   usuario1 => usuario1.Id,
                   (plantrabajos5, usuario1) => new { plantrabajos5, usuario1 })
               .GroupJoin(queryplantrabajoactividades,
                  plantrabajos6 => plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.Id,
                  plantrabajoactividad1 => plantrabajoactividad1.IdPlantrabajo,
                  (plantrabajos6, plantrabajoactividad1) => new { plantrabajos6, plantrabajoactividad1 })
               .SelectMany(x => x.plantrabajoactividad1.DefaultIfEmpty(), (plantrabajos6, plantrabajoactividad1) => new { plantrabajos6.plantrabajos6, plantrabajoactividad1 })
               .Select(x => new
               {
                   Id = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.Id,
                   Idanio = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.IdAnio,

                   titulo = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.titulo,
                   fullname = x.plantrabajos6.usuario1.FullName,
                   nombreareaacademica = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.areaacademica1.nombre,
                   fechaini = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.fechaini,
                   fechafin = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.fechafin,
                   activo = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.activo,
                   archivourl = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.archivourl,
                   estado = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.estado,
                   nombrefacultad = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.departamento1.nombrefacultad,
                   tipoplan = tipoplan,
                   observacioncomite = HttpUtility.HtmlEncode(x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.observacioncomite),
                   IdUser = x.plantrabajos6.usuario1.Id,
                   total = x.plantrabajoactividad1.total.ToString() == null ? "0" : x.plantrabajoactividad1.total.ToString(),
                   totalaprobado = x.plantrabajoactividad1.totalaprobado.ToString() == null ? "0" : x.plantrabajoactividad1.totalaprobado.ToString(),
                   totalobservado = x.plantrabajoactividad1.totalobservado.ToString() == null ? "0" : x.plantrabajoactividad1.totalobservado.ToString(),
                   totalenviado = x.plantrabajoactividad1.totalenviado.ToString() == null ? "0" : x.plantrabajoactividad1.totalenviado.ToString(),
                   coautor1 =  x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.coautor1,
                   coautor2 = x.plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.coautor2,
                   admin = admin,
               })
               .Where(x => x.Idanio.ToString() == searchanioValue);
               if (tipoplan == "C" || tipoplan=="F")
            {
                query = query
                      .Where(x =>(Guid.Parse(user.Id) == Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4") ? x.totalenviado != "0" || x.totalaprobado != "0" ||  x.totalenviado != "0" : x.totalenviado != "0"));


                if (!string.IsNullOrEmpty(searchValue))
                    query = query.Where(x => x.fullname.ToLower().Trim().Contains(searchValue.ToLower().Trim())  ||
                                            x.nombrefacultad.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                            );
            }
            else
            {
               
                query = query
                    .Where(x =>(Guid.Parse(user.Id) == Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4") ? "1"=="1" : x.IdUser == Guid.Parse(user.Id)));
            }
            Console.WriteLine(tipoplan);
               

            
            /*    .GroupJoin(queryplantrabajoactividades,
                    plantrabajos6 => plantrabajos6.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.Id,
                    plantrabajoactividad1 => plantrabajoactividad1.IdPlantrabajo,
                    (plantrabajos6, plantrabajoactividad1) => new { plantrabajos6, plantrabajoactividad1 })
                    */

            int recordsFiltered = await query.CountAsync();

            var data = await query

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
        public async Task<IActionResult> OnGetDatatableactividadesAsync(Guid id,string tipoplan="")
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionformativaPlantrabajoactividad, dynamic>> orderByPredicate = null;


            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.fechaini;
                    break;
                case "1":
                    orderByPredicate = (x) => x.fechaini;
                    break;
            }
            var admin = "";
            admin = "0";
            var user = await _userManager.GetUserAsync(User);

            if (Guid.Parse(user.Id) == Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4"))
            {
                admin = "1";
            }
                DateTime ahora = DateTime.Now;
            DateTime start = new DateTime(2010, 6, 14);
            var query = _context.InvestigacionformativaPlantrabajosactividades
                .AsNoTracking()
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Select(x => new
                {
                    dias = (DateTime.Parse(x.fechafin.ToString()) - DateTime.Parse(ahora.ToString("yyyy-MM-dd"))).Days,
                    fechaini = DateTime.Parse(x.fechaini.ToString()).ToString("dd/MM/yyyy"),
                    fechafin = DateTime.Parse(x.fechafin.ToString()).ToString("dd/MM/yyyy"),
                    IdPlantrabajo = x.IdPlantrabajo,
                    titulo = x.titulo,
                    archivourl = x.archivourl == null ? "" : x.archivourl,
                    estado = x.estado,
                    id = x.Id,
                    tipoplan = tipoplan,
                    archivoobservacionurl = x.archivoobservacionurl == null ? "" : x.archivoobservacionurl,
                    observacion = HttpUtility.HtmlEncode(x.observacion),
                   informefinal = x.informefinal,
                   anexourl = x.anexourl == null ? "" : x.anexourl,
                   ahora  = DateTime.Parse(x.fechafin.ToString()),
                   ahora2 = DateTime.Parse(x.fechafin.ToString()).ToString("yyyy-MM-dd"),
                    start= start,
                    admin=admin,
                })
                .Where(x => x.IdPlantrabajo == id && (tipoplan=="C"?x.informefinal=="0" : (tipoplan == "F" ? x.informefinal == "1":1==1)) );
            Console.WriteLine(id);
            int recordsFiltered = await query.CountAsync();


            var data = await   query.Skip(sentParameters.PagingFirstRecord)
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
        public async Task<IActionResult> OnPostEditFileActividadesAsync(InvestigacionformativaPlantrabajoactividadEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajosactividades = await _context.InvestigacionformativaPlantrabajosactividades.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajosactividades == null)
                return new BadRequestObjectResult("Sucedio un error");



            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigacionformativaplantrabajosactividades.archivourl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }
            investigacionformativaplantrabajosactividades.titulo = viewModel.titulo;
            investigacionformativaplantrabajosactividades.descripcion = viewModel.descripcion;
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnGetDetailFileActividadesAsync(Guid id)
        {
            var investigacionformativaplantrabajosactividades = await _context.InvestigacionformativaPlantrabajosactividades.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajosactividades == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionformativaplantrabajosactividades.Id,
                investigacionformativaplantrabajosactividades.titulo,
                investigacionformativaplantrabajosactividades.descripcion,

            };

            return new OkObjectResult(result);
        }
        public async Task<IActionResult> OnPostEditFileActividadesfinalAsync(InvestigacionformativaPlantrabajoactividadEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajosactividades = await _context.InvestigacionformativaPlantrabajosactividades.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajosactividades == null)
                return new BadRequestObjectResult("Sucedio un error");



            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigacionformativaplantrabajosactividades.archivourl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            if (viewModel.Fileanexo != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigacionformativaplantrabajosactividades.anexourl = await storage.UploadFile(viewModel.Fileanexo.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.PLANTRABAJO,
                        Path.GetExtension(viewModel.Fileanexo.FileName), FileStorageConstants.SystemFolder.INVESTIGACION_FORMATIVA);
            }
          
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnGetDetailFileActividadesfinalAsync(Guid id)
        {
            var investigacionformativaplantrabajosactividades = await _context.InvestigacionformativaPlantrabajosactividades.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajosactividades == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionformativaplantrabajosactividades.Id,
              

            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnPostEditobservacionactividadAsync(InvestigacionformativaPlantrabajoactividadEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajosactividades = await _context.InvestigacionformativaPlantrabajosactividades.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajosactividades == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (viewModel.estado == "1")
            {
                investigacionformativaplantrabajosactividades.estado = "2";
            }
            else
            {
                investigacionformativaplantrabajosactividades.estado = "3";

            }
            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                investigacionformativaplantrabajosactividades.archivoobservacionurl = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.INVESTIGATIONCONVOCATION_DOCUMENTS,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.TEACHER_INVESTIGATION);
            }

            investigacionformativaplantrabajosactividades.observacion = viewModel.observacion;
            await _context.SaveChangesAsync();

            var investigacionformativaplantrabajosactividadeshistoriales = new InvestigacionformativaPlantrabajoactividadhistorial
            {
                IdPlantrabajoactividad = viewModel.Id,
                observacion = viewModel.observacion,
                estado = viewModel.estado,
                archivourl = investigacionformativaplantrabajosactividades.archivoobservacionurl,
            };

            await _context.InvestigacionformativaPlantrabajosactividadeshistoriales.AddAsync(investigacionformativaplantrabajosactividadeshistoriales);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }


        public async Task<IActionResult> OnPostEnviarPlanactividadAsync(InvestigacionformativaPlantrabajoactividadEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajosactividades = await _context.InvestigacionformativaPlantrabajosactividades.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajosactividades == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigacionformativaplantrabajosactividades.estado = "1";
            await _context.SaveChangesAsync();

            var investigacionformativaplantrabajoshistoriales = new InvestigacionformativaPlantrabajoactividadhistorial
            {
                IdPlantrabajoactividad = viewModel.Id,
                observacion = "Enviado para revisión",
                estado = "1",
            };

            await _context.InvestigacionformativaPlantrabajosactividadeshistoriales.AddAsync(investigacionformativaplantrabajoshistoriales);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }

    }
}