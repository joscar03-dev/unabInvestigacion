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
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaAsesoriaestructuraalumnoViewModels;
using AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionAsesoria.InvestigacionasesoriaEstructurainvestigacionViewModels;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing.Diagrams;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.InvestigacionasesoriaAsesoriadocentedetallePage
{
    [Authorize(Roles = GeneralConstants.ROLES.DOCENTE_ASESOR + "," +
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
                 })
                 .Where(x => x.Id == Guid.Parse(Id)).ToList();

            foreach (var Item in query)
            {
                ViewData["tipo"] = Item.nombre;
                ViewData["idtipo"] = Item.IdTipo;

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

        public async Task<IActionResult> OnPostEditAsync(InvestigacionasesoriaAsesoriaestructuraEditViewModel viewModel)
        {

            if (viewModel.Id== Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                var investigacionasesoriaasesoriaestructura = new InvestigacionasesoriaAsesoriaestructura
                {
                    IdEstructura = viewModel.IdEstructura,
                    IdAsesoria = viewModel.IdAsesoria,
                    fechaini = ConvertHelpers.DatepickerToDatetime(viewModel.fechaini),
                    fechafin = ConvertHelpers.DatepickerToDatetime(viewModel.fechafin),
                    
                };

                await _context.InvestigacionasesoriaAsesoriasestructuras.AddAsync(investigacionasesoriaasesoriaestructura);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                var investigacionasesoriaasesoriaestructura = await _context.InvestigacionasesoriaAsesoriasestructuras.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();


                investigacionasesoriaasesoriaestructura.IdEstructura = viewModel.IdEstructura;
                investigacionasesoriaasesoriaestructura.IdAsesoria = viewModel.IdAsesoria;
                investigacionasesoriaasesoriaestructura.fechaini = ConvertHelpers.DatepickerToDatetime(viewModel.fechaini);
                investigacionasesoriaasesoriaestructura.fechafin = ConvertHelpers.DatepickerToDatetime(viewModel.fechafin);
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

            Console.WriteLine(idtipo);
            Console.WriteLine(idasesoria);
            var sentParameters = _dataTablesService.GetSentParameters();
            Expression<Func<InvestigacionasesoriaAsesoria, dynamic>> orderByPredicate = null;
            Console.WriteLine(idtipo);
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = (x) => x.Id;
                    break;
                case "1":
                    orderByPredicate = (x) => x.nroresolucion;
                    break;
            }

           

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
                .Select(x => new
                {
                    IdEstructura= x.estructura1.Id,
                    nombre =  x.estructura1.nombre,
                    codigo = x.estructura1.codigo,
                    fechaini = x.asesoriaestructura1.fechaini,
                    fechafin =  x.asesoriaestructura1.fechafin,
                    idtipoestructura = x.estructura1.IdTipotrabajoinvestigacion,
                    id = x.asesoriaestructura1.Id.ToString(),

                })
                .Where(x=>x.idtipoestructura == Guid.Parse(idtipo) )
                .OrderBy(x => x.codigo)
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
        public async Task<IActionResult> OnGetDatatableactividadesAsync(string id = null)
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

            var user = await _userManager.GetUserAsync(User);

            var queryalumno = _context.MaestroAlumnos.AsNoTracking();
            var queryasesoriaestructuraalumno = _context.InvestigacionasesoriaAsesoriasestructurasalumnos.AsNoTracking();
            if (string.IsNullOrEmpty(id))
                id = "00000000-0000-0000-0000-000000000000";

            
             queryasesoriaestructuraalumno = queryasesoriaestructuraalumno.Where(x => x.IdAsesoriaestructura == Guid.Parse(id));

            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            Console.WriteLine(id);
            Console.WriteLine("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

            var queryusuario = _context.MaestroUsuarios.AsNoTracking();
            var queryasesoriaestructura = _context.InvestigacionasesoriaAsesoriasestructuras.AsNoTracking();
            var query = _context.InvestigacionasesoriaEstructurainvestigaciones.AsNoTracking();

            Console.WriteLine(id);


            Console.WriteLine("bbbbbb");
            var querynuevo = queryasesoriaestructuraalumno
                 .Join(queryalumno,
                  estructura1 => estructura1.IdAlumno,
                  alumno1 => alumno1.Id,
                  (estructura1, alumno1) => new { estructura1, alumno1 })
                 .Join(queryusuario,
                 estructura2 => estructura2.alumno1.IdUser,
                 usuario1 => usuario1.Id,
                 (estructura2, usuario1) => new { estructura2, usuario1 })
                 .Select(x => new
                 {
                     idAsesoriaestructuraalumno = x.estructura2.estructura1.Id,
                     nombre = x.usuario1.FullName,
                     idAlumno = x.estructura2.estructura1.IdAlumno,
                     rutaarchivo = x.estructura2.estructura1.rutaarchivo ?? "",
                     id = x.estructura2.estructura1.Id.ToString(),
                     estado = x.estructura2.estructura1.estado,
                     observacion = x.estructura2.estructura1.observacion,


                 }).AsNoTracking();
           

            Console.WriteLine("cccccccc");
            var data = await querynuevo
                .Skip(sentParameters.PagingFirstRecord)
                .Take(sentParameters.RecordsPerDraw)
                .ToListAsync();
            Console.WriteLine("dddddddd");

            int recordsFiltered = await querynuevo.CountAsync();


            int recordsTotal = data.Count;
            Console.WriteLine(recordsTotal);
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


            Console.WriteLine("aaaaaaa");
            var queryasesoriaestructuraalumno = _context.InvestigacionasesoriaAsesoriasestructurasalumnos.Where(x => x.Id == Guid.Parse(id)).AsNoTracking();
            var queryasesoriaaseroriaestructura = _context.InvestigacionasesoriaAsesoriasestructuras.AsNoTracking();
            var queryasesoriaestructurarequisito =  _context.InvestigacionasesoriaEstructurainvestigacionesrequisitos.AsNoTracking();
            var queryasesoriaestructura = _context.InvestigacionasesoriaEstructurainvestigaciones.AsNoTracking();
          
        
            int recordsFiltered = await queryasesoriaestructuraalumno.CountAsync();
         


            var data = await queryasesoriaestructuraalumno
                 .Join(queryasesoriaaseroriaestructura,
                  estructuraalumno1 => estructuraalumno1.IdAsesoriaestructura,
                  asesoriaestructura1 => asesoriaestructura1.Id,
                  (estructuraalumno1, asesoriaestructura1) => new { estructuraalumno1, asesoriaestructura1 })
                 .Join(queryasesoriaestructura,
                  asesoriaestructura2=>asesoriaestructura2.asesoriaestructura1.IdEstructura,
                  estructura1=>estructura1.Id,
                  (asesoriaestructura2, estructura1) => new { asesoriaestructura2, estructura1 }) 
                 .Join(queryasesoriaestructurarequisito,
                 estructura2=>estructura2.estructura1.Id,
                 estructurarequisito1=>estructurarequisito1.Idestructurainvestigacion,
                 (estructura2, estructurarequisito1) => new { estructura2, estructurarequisito1 })
                
               .Select(

                    x => new
                    {
                        id= x.estructurarequisito1.Id,
                        descripcion = x.estructurarequisito1.descripcion,
                        orden = x.estructurarequisito1.orden,
                        nombreestructura=x.estructura2.estructura1.nombre,
                      
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
        public async Task<IActionResult> OnPostEditobservacionactividadAsync(InvestigacionasesoriaAsesoriaestructuraalumnoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Revise los campos del formulario");

            int c = viewModel.observacion.Count();
            string observacion = "";
            string banrequisito = "";
            string idrequisito = "";
            string estado = "1";
            for (int i = 0; i < c; i++)
            {
                banrequisito = viewModel.banRequisito[i];
                if (banrequisito==null)
                {
                    return new BadRequestObjectResult("Revise los campos del formulario");
                }
            }
                for (int i = 0; i < c; i++)
            {
                observacion =  viewModel.observacion[i];
                banrequisito = viewModel.banRequisito[i];
                idrequisito = viewModel.idRequisito[i];
                if (banrequisito == "0")
                {
                    estado = "0";
                }
                var investigacionasesoriaasesoriaestructuraalumnoobservacion = new InvestigacionasesoriaAsesoriaestructuraalumnoobservacion
                {
                    IdAsesoriaestructuraalumno = viewModel.Id,
                    observaciones = observacion,
                    estado  = banrequisito,
                    IdEstructurarequisito = Guid.Parse(idrequisito),
                    vigente ="1",
                   
                };

                await _context.InvestigacionasesoriaAsesoriasestructurasalumnosobservaciones .AddAsync(investigacionasesoriaasesoriaestructuraalumnoobservacion);
                await _context.SaveChangesAsync();

            }
            Console.WriteLine(c);
            var investigacionasesoriaasesoriaestructuraalumnno = await _context.InvestigacionasesoriaAsesoriasestructurasalumnos.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionasesoriaasesoriaestructuraalumnno == null)
                return new BadRequestObjectResult("Sucedio un error");

            if (estado == "1")
            {
                investigacionasesoriaasesoriaestructuraalumnno.estado = "2";
                investigacionasesoriaasesoriaestructuraalumnno.observacion = "Aprobado";
            }
            else
            {
                investigacionasesoriaasesoriaestructuraalumnno.estado = "3";
                investigacionasesoriaasesoriaestructuraalumnno.observacion = "Observado";


            }
            
            await _context.SaveChangesAsync();

            return new OkObjectResult("");
        }


    }
}
