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
using System.Web;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionFormativa.InvestigacionformativaPlantrabajoPage
{
    [Authorize(Roles = GeneralConstants.ROLES.EVALUATOR_COMMITTEE + "," + 
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
        public async Task OnGetTipoPlanAdmininstradorAsync()
        {
            string tipoplan = "A";
            string nombretipoplan = "Administrador";
            ViewData["idanio"] = "";
            ViewData["anio"] = "";
            var investigacionformativaconfiguraranios = await _context.InvestigacionformativaConfiguracionanios.Where(x => x.activo == "1").FirstOrDefaultAsync();
            if (investigacionformativaconfiguraranios != null)
            {
                String anio = investigacionformativaconfiguraranios.nombre;
                Guid idanio = investigacionformativaconfiguraranios.Id;
                ViewData["idanio"] = idanio;
                ViewData["anio"] = anio;

            }
            ViewData["tipoplan"] = tipoplan;
            ViewData["nombretipoplan"] = nombretipoplan;
            
        }
        public void OnGetTipoPlanComite()
        {
            string tipoplan = "C";
            string nombretipoplan = "Comite";

            ViewData["tipoplan"] = tipoplan;
            ViewData["nombretipoplan"] = nombretipoplan;

        }

        public InvestigacionformativaPlantrabajoCreateViewModel input { get; set; }
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
                objetivo=viewModel.objetivo,
                descripcion = viewModel.descripcion,
                IdLinea=viewModel.IdLinea,
                fechafin=viewModel.fechafin,
                fechaini=viewModel.fechaini,
                IdTipoevento=viewModel.IdTipoevento,
                IdTiporesultado=viewModel.IdTiporesultado,
                estado="0",
                archivourl="",
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

            var user = await _userManager.GetUserAsync(User);
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
                investigacionformativaplantrabajos.activo ,
                user.UserName,
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
                observacion ="Enviado para revisión",               
                estado = "1",
            };

            await _context.InvestigacionformativaPlantrabajoshistoriales.AddAsync(investigacionformativaplantrabajoshistoriales);
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }
        public async Task<IActionResult> OnGetDetailPlanObservacionAsync(Guid id)
        {
            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionformativaplantrabajos.Id,
                
            };

            return new OkObjectResult(result);
        }
        
        public async Task<IActionResult> OnPostEditobservacionAsync(InvestigacionformativaPlantrabajoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionformativaplantrabajos = await _context.InvestigacionformativaPlantrabajos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionformativaplantrabajos == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (viewModel.estado == "1") {
                investigacionformativaplantrabajos.estado = "2";
            }
            else
            {
                investigacionformativaplantrabajos.estado = "3";

            }
          
            investigacionformativaplantrabajos.observacioncomite = viewModel.observacion;
            await _context.SaveChangesAsync();

            var investigacionformativaplantrabajoshistoriales = new InvestigacionformativaPlantrabajohistorial
            {
                IdPlantrabajo = viewModel.Id,
                observacion = viewModel.observacion,
                estado = viewModel.estado,
            };

            await _context.InvestigacionformativaPlantrabajoshistoriales.AddAsync(investigacionformativaplantrabajoshistoriales);
            await _context.SaveChangesAsync();

            if (viewModel.estado == "1")
            {
                DateTime d1 = new DateTime(2023, 06, 20,0, 0, 0);
                DateTime d2 = new DateTime(2023, 11, 13, 0, 0, 0);
                DateTime d3 = new DateTime(2023, 06, 20, 0, 0, 0);
                while (d1 <= d2)
                {
                  
                    d3 = d1.AddMonths(1);

                    if(d3 > d2)
                    {
                        d3 = d2;
                    }
                  
                    var investigacionformativaplantrabajosactividades = new InvestigacionformativaPlantrabajoactividad
                    {
                        IdPlantrabajo = viewModel.Id,
                        fechaini = d1,
                        fechafin = d3,
                        estado= "0",
                        informefinal ="0",

                    };
                    await _context.InvestigacionformativaPlantrabajosactividades.AddAsync(investigacionformativaplantrabajosactividades);
                    await _context.SaveChangesAsync();
                    d1 = d1.AddMonths(1);                 


                }
                var investigacionformativaplantrabajosactividades2 = new InvestigacionformativaPlantrabajoactividad
                {
                    IdPlantrabajo = viewModel.Id,
                    fechaini = d3,
                    fechafin = d3,
                    estado = "0",
                    informefinal = "1",
                    titulo = "INFORME FINAL",

                };
                await _context.InvestigacionformativaPlantrabajosactividades.AddAsync(investigacionformativaplantrabajosactividades2);
                await _context.SaveChangesAsync();

            }
            return new OkObjectResult("");
        }



        
        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, string tipoplan = null, string searchanioValue = null)
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
            if (querycomite != null)           {


                 Idfacultad = querycomite.IdFacultad;
                 
            }

            var queryareaacademicas = _context.MaestroAreasacademicas.AsNoTracking();
            var querylineas = _context.MaestroLineas.AsNoTracking();
            var querydocentes = _context.MaestroDocentes.AsNoTracking();
            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();

            

            var queryfacultades = _context.MaestroFacultades.AsNoTracking();
            var querydepartamentos = _context.MaestroDepartamentos
                .AsNoTracking()
                .Join(queryfacultades,
                departamentos1=>departamentos1.IdFacultad,
                facultades1=>facultades1.Id,
                (departamentos1, facultades1) => new { departamentos1, facultades1 })
                .Select(
                    x => new
                    {
                        Id = x.departamentos1.Id,
                        nombre= x.departamentos1.nombre,
                        nombrefacultad= x.facultades1.nombre,
                        IdFacultad = x.facultades1.Id,
                    }
                );


            var query = _context.InvestigacionformativaPlantrabajos
                .AsNoTracking();
            if (tipoplan == "C")
            {
                querydepartamentos = querydepartamentos.Where(x=> x.IdFacultad == Idfacultad);
                 query = query.Where(x => x.estado=="1");
            }

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.titulo.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.titulo.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
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
                .Select(x => new
                {
                    Id =  x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.Id,
                    Idanio = x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.IdAnio,
                    titulo =x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.titulo,
                    fullname = x.usuario1.FullName,
                    nombreareaacademica =  x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.areaacademica1.nombre,
                    fechaini =   x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.fechaini,
                    fechafin = x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.fechafin,
                    activo = x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.activo,
                    archivourl = x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.archivourl,
                    estado = x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.estado,
                    nombrefacultad = x.plantrabajos5.plantrabajos4.plantrabajos3.departamento1.nombrefacultad,
                    tipoplan= tipoplan,
                    observacioncomite = HttpUtility.HtmlEncode(x.plantrabajos5.plantrabajos4.plantrabajos3.plantrabajos2.plantrabajos1.observacioncomite),
                    userName = user.UserName,
                })
                .Where(x=> x.Idanio.ToString() == searchanioValue)
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