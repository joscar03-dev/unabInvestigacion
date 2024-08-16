using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Options;
using System.IO;
using AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria;
using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraalumnoViewModels;


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
using DocumentFormat.OpenXml.Office.CustomUI;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.DOMAIN.Entities.InvestigacionFormativa;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.investigacionFormativa.InvestigacionformativaPlantrabajoViewModels;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaalumnodetallePage
{
    [Authorize(Roles = GeneralConstants.ROLES.STUDENTS + "," + 
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
            _context = context;
            _dataTablesService = dataTablesService;
            _storageCredentials = storageCredentials;
            _userManager = userManager;

        }
        public void OnGet(String Id)
        {
            var querytipoinvestigacion = _context.InvestigacionasesoriaTipotrabajoinvestigaciones.AsNoTracking();
            var query = _context.InvestigacionasesoriaAsesorias
                 .Join(querytipoinvestigacion,
                   asesoria1=>asesoria1.IdTipotrabajoinvestigacion,
                   tipoasesoria1=>tipoasesoria1.Id,
                   (asesoria1, tipoasesoria1) => new { asesoria1, tipoasesoria1 } )
                 .Select(x => new
                 {
                     Id = x.asesoria1.Id,
                     IdTipo =x.tipoasesoria1.Id,
                     nombre= x.tipoasesoria1.nombre,
                     codigo = x.tipoasesoria1.codigo,
                 })
                 .Where(x => x.Id == Guid.Parse(Id)).ToList();

            foreach (var Item in query)
            {
                ViewData["tipo"] = Item.nombre;
                ViewData["idtipo"] = Item.IdTipo;
                ViewData["codTipoinvestigacion"] = Item.codigo;

            }
            ViewData["idasesoria"] = Id;
        }

        public InvestigacionasesoriaAsesoriaCreateViewModel fechaini { get; set; }
        public InvestigacionasesoriaAsesoriaCreateViewModel fechafin { get; set; }

        public async Task<IActionResult> OnPostCreateAsync(InvestigacionasesoriaAsesoriaCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionasesoriaasesoria = new InvestigacionasesoriaAsesoria
                            {
                nroresolucion = viewModel.nroresolucion,
                IdCarrera=viewModel.IdCarrera,
                IdAlumno = viewModel.IdAlumno,
                IdAlumno2 = viewModel.IdAlumno2,
                IdAsesor = viewModel.IdAsesor,
                fechaini = ConvertHelpers.DatepickerToDatetime(viewModel.fechaini),
                fechafin = ConvertHelpers.DatepickerToDatetime(viewModel.fechafin),
                activo = viewModel.activo ?? "0",
                IdTipotrabajoinvestigacion = viewModel.IdTipotrabajoinvestigacion,
                IdAnio = viewModel.IdAnio,
            };

            await _context.InvestigacionasesoriaAsesorias.AddAsync(investigacionasesoriaasesoria);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostEditAsync(InvestigacionasesoriaAsesoriaestructuraalumnoEditViewModel viewModel)
        {

            var rutaarchivo = "";
            var nombrearchivo = "";
            if (viewModel.File != null)
            {
                var storage = new CloudStorageService(_storageCredentials);

                rutaarchivo = await storage.UploadFile(viewModel.File.OpenReadStream(), FileStorageConstants.CONTAINER_NAMES.ASESORIAALUMNO,
                        Path.GetExtension(viewModel.File.FileName), FileStorageConstants.SystemFolder.ASESORIA);
                nombrearchivo = viewModel.File.FileName;
            }

            if (viewModel.Id== Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                var investigacionasesoriaasesoriaestructuraalumno = new InvestigacionasesoriaAsesoriaestructuraalumno
                {
                    IdAlumno = viewModel.IdAlumno,
                    IdAsesoriaestructura = viewModel.IdAsesoriaestructura,
                    descripcion = viewModel.descripcion,
                    nombrearchivo = nombrearchivo,
                    rutaarchivo = rutaarchivo,
                    estado = "0",
                    
                };

                await _context.InvestigacionasesoriaAsesoriasestructurasalumnos.AddAsync(investigacionasesoriaasesoriaestructuraalumno);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                var investigacionasesoriaasesoriaestructuraalumnno = await _context.InvestigacionasesoriaAsesoriasestructurasalumnos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();


                investigacionasesoriaasesoriaestructuraalumnno.IdAlumno = viewModel.IdAlumno;
                investigacionasesoriaasesoriaestructuraalumnno.IdAsesoriaestructura = viewModel.IdAsesoriaestructura;
                investigacionasesoriaasesoriaestructuraalumnno.descripcion = viewModel.descripcion;
                investigacionasesoriaasesoriaestructuraalumnno.estado = "0";
                investigacionasesoriaasesoriaestructuraalumnno.nombrearchivo = nombrearchivo;
                investigacionasesoriaasesoriaestructuraalumnno.rutaarchivo = rutaarchivo;
                 await _context.SaveChangesAsync();


            }
         
           
         

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionasesoriaasesoria = await _context.InvestigacionasesoriaAsesorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriaasesoria == null) return new OkResult();

            _context.InvestigacionasesoriaAsesorias.Remove(investigacionasesoriaasesoria);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id )
        {
            var investigacionasesoriaasesoriaestructura = await _context.InvestigacionasesoriaAsesoriasestructuras.Where(x => x.Id ==id).FirstOrDefaultAsync();
            
            if (investigacionasesoriaasesoriaestructura == null) return new OkResult();

            var result = new
            {
                fechafin = DateTime.Parse(investigacionasesoriaasesoriaestructura.fechafin.ToString()).ToString("dd/MM/yyyy"),//.ToString().ToString("dd/MM/yyyy"),
               fechaini = DateTime.Parse(investigacionasesoriaasesoriaestructura.fechaini.ToString()).ToString("dd/MM/yyyy"),//.ToString("dd/MM/yyyy"),
               id= investigacionasesoriaasesoriaestructura.Id,
            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string idtipo = null, String idasesoria=null)
        {

            var sentParameters = _dataTablesService.GetSentParameters();
           

            var user = await _userManager.GetUserAsync(User);

            var queryalumno = _context.MaestroAlumnos.Where(x=>x.IdUser== Guid.Parse(user.Id) || Guid.Parse(user.Id) == Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4")).ToList();

            var idalumno = "";
            foreach (var Item in queryalumno)
            {
                idalumno = Item.Id.ToString();

            }

            var queryasesoriaestructuraalumno = _context.InvestigacionasesoriaAsesoriasestructurasalumnos.Where(x=>x.IdAlumno == Guid.Parse(idalumno) ).AsNoTracking();
            var queryasesoriaestructura = _context.InvestigacionasesoriaAsesoriasestructuras.Where(x=>x.IdAsesoria==Guid.Parse(idasesoria)).AsNoTracking();
            var query = _context.InvestigacionasesoriaEstructurainvestigaciones.AsNoTracking();
            

            Console.WriteLine("rr");
            Console.WriteLine(idtipo);
            Console.WriteLine(idasesoria);
            int recordsFiltered = await query.CountAsync();

            var data = await query
                 .GroupJoin(queryasesoriaestructura,
                  estructura1 => estructura1.Id,
                  asesoriaestructura1 => asesoriaestructura1.IdEstructura,
                  (estructura1, asesoriaestructura1) => new { estructura1, asesoriaestructura1 })
                .SelectMany(x => x.asesoriaestructura1.DefaultIfEmpty(), (estructura1, asesoriaestructura1) => new { estructura1.estructura1, asesoriaestructura1 })
                .GroupJoin(queryasesoriaestructuraalumno,
                 asesoriaestructura2 => asesoriaestructura2.asesoriaestructura1.Id,
                 asesoriaestructuraalumno1 => asesoriaestructuraalumno1.IdAsesoriaestructura,
                 (asesoriaestructura2, asesoriaestructuraalumno1) => new { asesoriaestructura2, asesoriaestructuraalumno1 })
                .SelectMany(x => x.asesoriaestructuraalumno1.DefaultIfEmpty(), (asesoriaestructura2, asesoriaestructuraalumno1) => new { asesoriaestructura2.asesoriaestructura2, asesoriaestructuraalumno1 })
                .Select(x => new
                {
                    IdEstructura = x.asesoriaestructura2.estructura1.Id,
                    nombre = x.asesoriaestructura2.estructura1.nombre,
                    codigo = x.asesoriaestructura2.estructura1.codigo,
                    fechaini = x.asesoriaestructura2.asesoriaestructura1.fechaini,
                    fechafin = x.asesoriaestructura2.asesoriaestructura1.fechafin,
                    idtipoestructura = x.asesoriaestructura2.estructura1.IdTipotrabajoinvestigacion,
                    idAsesoriaestructura = x.asesoriaestructura2.asesoriaestructura1.Id.ToString(),
                    idAlumno = idalumno,
                    rutaarchivo = x.asesoriaestructuraalumno1.rutaarchivo ?? "",
                    id = x.asesoriaestructuraalumno1.Id.ToString(),
                    estado = x.asesoriaestructuraalumno1.estado,
                    observacion = x.asesoriaestructuraalumno1.observacion,


                })                
                .Where(x=>x.idtipoestructura == Guid.Parse(idtipo) )
                .OrderBy(x=>x.codigo)
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

        public async Task<IActionResult> OnGetDatatableactividadesobservacionesAsync(string id = null)
        {


            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionasesoriaAsesoria, dynamic>> orderByPredicate = null;

            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nroresolucion;
                    break;
            }

            var queryasesoriaestructuraalumnoobservacion = _context.InvestigacionasesoriaAsesoriasestructurasalumnosobservaciones.Where(x=>x.IdAsesoriaestructuraalumno == Guid.Parse(id) && x.vigente=="1").AsNoTracking(); 
            var queryasesoriaestructuraalumno = _context.InvestigacionasesoriaAsesoriasestructurasalumnos.Where(x => x.Id == Guid.Parse(id)).AsNoTracking();
            var queryasesoriaaseroriaestructura = _context.InvestigacionasesoriaAsesoriasestructuras.AsNoTracking();
            var queryasesoriaestructurarequisito = _context.InvestigacionasesoriaEstructurainvestigacionesrequisitos.AsNoTracking();
            var queryasesoriaestructura = _context.InvestigacionasesoriaEstructurainvestigaciones.AsNoTracking();


            int recordsFiltered = await queryasesoriaestructuraalumno.CountAsync();



            var data = await queryasesoriaestructuraalumno
                 .Join(queryasesoriaaseroriaestructura,
                  estructuraalumno1 => estructuraalumno1.IdAsesoriaestructura,
                  asesoriaestructura1 => asesoriaestructura1.Id,
                  (estructuraalumno1, asesoriaestructura1) => new { estructuraalumno1, asesoriaestructura1 })
                 .Join(queryasesoriaestructura,
                  asesoriaestructura2 => asesoriaestructura2.asesoriaestructura1.IdEstructura,
                  estructura1 => estructura1.Id,
                  (asesoriaestructura2, estructura1) => new { asesoriaestructura2, estructura1 })
                 .Join(queryasesoriaestructurarequisito,
                 estructura2 => estructura2.estructura1.Id,
                 estructurarequisito1 => estructurarequisito1.Idestructurainvestigacion,
                 (estructura2, estructurarequisito1) => new { estructura2, estructurarequisito1 })
                 .Join(queryasesoriaestructuraalumnoobservacion,
                 estructurarequisito2 => estructurarequisito2.estructurarequisito1.Id,
                 estructurarequisitoobservacion1=> estructurarequisitoobservacion1.IdEstructurarequisito,
                 (estructurarequisito2, estructurarequisitoobservacion1) => new { estructurarequisito2, estructurarequisitoobservacion1 })

               .Select(

                    x => new
                    {
                        id = x.estructurarequisito2.estructurarequisito1.Id,
                        descripcion = x.estructurarequisito2.estructurarequisito1.descripcion,
                        orden = x.estructurarequisito2.estructurarequisito1.orden,
                        nombreestructura = x.estructurarequisito2.estructura2.estructura1.nombre,
                        estado = x.estructurarequisitoobservacion1.estado,
                        observacion = x.estructurarequisitoobservacion1.observaciones,

                    }
                )
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

        public async Task<IActionResult> OnPostEnviarTrabajoAsync(InvestigacionasesoriaAsesoriaestructuraalumnoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            var investigacionasesoriaasesoriaestructuraalumnno = await _context.InvestigacionasesoriaAsesoriasestructurasalumnos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionasesoriaasesoriaestructuraalumnno == null)
                return new BadRequestObjectResult("Sucedio un error");

            investigacionasesoriaasesoriaestructuraalumnno.estado = "1";
            await _context.SaveChangesAsync();

           /* var investigacionformativaplantrabajoshistoriales = new InvestigacionformativaPlantrabajohistorial
            {
                IdPlantrabajo = viewModel.Id,
                observacion = "Enviado para revisión",
                estado = "1",
            };

            await _context.InvestigacionformativaPlantrabajoshistoriales.AddAsync(investigacionformativaplantrabajoshistoriales);
            await _context.SaveChangesAsync();*/

            return new OkObjectResult("");
        }

    }
}
