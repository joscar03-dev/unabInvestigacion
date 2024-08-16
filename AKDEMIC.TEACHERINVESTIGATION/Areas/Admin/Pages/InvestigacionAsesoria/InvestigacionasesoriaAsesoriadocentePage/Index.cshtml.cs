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
using AKDEMIC.DOMAIN.Entities.InvestigacionAsesoria;
using AKDEMIC.CORE.Helpers;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigacionAsesoria.InvestigacionasesoriaAsesoriadocentePage
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
        public void OnGet()
        {
        }
        public void OnGetAsesoriaProyecto()
        {
            ViewData["idTipoinvestigacion"] = "08dbd9d4-f9ea-4669-8fcb-702fe80260c1";
            ViewData["codTipoinvestigacion"] = "02";


        }
        public void OnGetAsesoriaInforme()
        {
            ViewData["idTipoinvestigacion"] = "08db9dd8-3508-42a8-899d-217aaa449dee";
            ViewData["codTipoinvestigacion"] = "01";


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

        public async Task<IActionResult> OnPostEditAsync(InvestigacionasesoriaAsesoriaEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return new BadRequestObjectResult("Verificar el formulario");

            var investigacionasesoriaasesoria = await _context.InvestigacionasesoriaAsesorias.Where(x => x.Id == viewModel.Id).FirstOrDefaultAsync();

            if (investigacionasesoriaasesoria == null) return new BadRequestObjectResult("Sucedio un error");

            investigacionasesoriaasesoria.IdCarrera = viewModel.IdCarrera;
            investigacionasesoriaasesoria.IdAlumno = viewModel.IdAlumno;
            investigacionasesoriaasesoria.IdAsesor = viewModel.IdAsesor;
            investigacionasesoriaasesoria.Id = viewModel.Id;
            investigacionasesoriaasesoria.nroresolucion = viewModel.nroresolucion;
            investigacionasesoriaasesoria.fechaini = viewModel.fechaini;
            investigacionasesoriaasesoria.fechafin = viewModel.fechafin;
            investigacionasesoriaasesoria.IdAlumno2 = viewModel.IdAlumno2;
            investigacionasesoriaasesoria.activo = viewModel.activo ?? "0";
            investigacionasesoriaasesoria.IdTipotrabajoinvestigacion = viewModel.IdTipotrabajoinvestigacion;
            investigacionasesoriaasesoria.IdAnio = viewModel.IdAnio;
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            var investigacionasesoriaasesoria = await _context.InvestigacionasesoriaAsesorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriaasesoria == null) return new BadRequestObjectResult("Sucedio un error");

            _context.InvestigacionasesoriaAsesorias.Remove(investigacionasesoriaasesoria);
            await _context.SaveChangesAsync();
            return new OkResult();
        }


        public async Task<IActionResult> OnGetDetailAsync(Guid id)
        {
            var investigacionasesoriaasesoria = await _context.InvestigacionasesoriaAsesorias.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (investigacionasesoriaasesoria == null) return new BadRequestObjectResult("Sucedio un error");

            var result = new
            {
                investigacionasesoriaasesoria.Id,
                investigacionasesoriaasesoria.nroresolucion,
               fechafin= DateTime.Parse(investigacionasesoriaasesoria.fechafin.ToString()).ToString("dd/MM/yyyy"),
                fechaini= DateTime.Parse(investigacionasesoriaasesoria.fechaini.ToString()).ToString("dd/MM/yyyy"),
                investigacionasesoriaasesoria.IdCarrera,
                investigacionasesoriaasesoria.IdAsesor,
                investigacionasesoriaasesoria.IdAlumno,              
                investigacionasesoriaasesoria.activo ,
                investigacionasesoriaasesoria.IdTipotrabajoinvestigacion,
                investigacionasesoriaasesoria.IdAlumno2,
                investigacionasesoriaasesoria.IdAnio,

            };

            return new OkObjectResult(result);
        }

        public async Task<IActionResult> OnGetDatatableAsync(string searchValue = null, string searchIdtipo = null)
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


            var queryanio = _context.MaestroAnios.AsNoTracking();

            var querycarrera = _context.MaestroCarreras.AsNoTracking();
            var queryusuarios = _context.MaestroUsuarios.AsNoTracking();

            Console.WriteLine(user.Id);
            var queryalumno = _context.MaestroAlumnos.AsNoTracking();
            var querydocente = _context.InvestigacionasesoriaAsesores.AsNoTracking();
            var querytipotrabajoinvestigacion = _context.InvestigacionasesoriaTipotrabajoinvestigaciones
                .Where(x => x.Id == Guid.Parse(searchIdtipo))
                .AsNoTracking();

            var query = _context.InvestigacionasesoriaAsesorias
                .AsNoTracking();

            if (!string.IsNullOrEmpty(searchValue))
                query = query.Where(x => x.nroresolucion.ToLower().Trim().Contains(searchValue.ToLower().Trim()) ||
                                    x.nroresolucion.ToLower().Trim().Contains(searchValue.ToLower().Trim()));

            int recordsFiltered = await query.CountAsync();

            var data = await query
                .OrderByCondition(sentParameters.OrderDirection, orderByPredicate)
                .Join(querycarrera,
                    asesoria1 => asesoria1.IdCarrera,
                    carrera1 => carrera1.Id,
                     (asesoria1, carrera1) => new { asesoria1, carrera1 })
               .Join(querydocente,
                    asesoria2 => asesoria2.asesoria1.IdAsesor,
                    docente1 => docente1.Id,
                    (asesoria2, docente1) => new { asesoria2, docente1 })
               .Join(queryusuarios,
                     asesoria3 => asesoria3.docente1.IdUser,
                     usuario1 => usuario1.Id,
                      (asesoria3, usuario1) => new { asesoria3, usuario1 })
               .Join(queryalumno,
                     asesoria4 => asesoria4.asesoria3.asesoria2.asesoria1.IdAlumno,
                     alumno1 => alumno1.Id,
                       (asesoria4, alumno1) => new { asesoria4, alumno1 })
                .Join(queryusuarios,
                     asesoria5 => asesoria5.alumno1.IdUser,
                     usuario2 => usuario2.Id,
                      (asesoria5, usuario2) => new { asesoria5, usuario2 })
                .Join(querytipotrabajoinvestigacion,
                      asesoria6 => asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.IdTipotrabajoinvestigacion,
                      tipo1 => tipo1.Id,
                      (asesoria6, tipo1) => new { asesoria6, tipo1 })
                 .GroupJoin(queryalumno,
                  asesoria7 => asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.IdAlumno2,
                  alumno2 => alumno2.Id,
                  (asesoria7, alumno2) => new { asesoria7, alumno2 })
               .SelectMany(x => x.alumno2.DefaultIfEmpty(), (asesoria7, alumno2) => new { asesoria7.asesoria7, alumno2 })
                .GroupJoin(queryusuarios,
                  asesoria8 => asesoria8.alumno2.IdUser,
                  usuario3 => usuario3.Id,
                  (asesoria8, usuario3) => new { asesoria8, usuario3 })
               .SelectMany(x => x.usuario3.DefaultIfEmpty(), (asesoria8, usuario3) => new { asesoria8.asesoria8, usuario3 })
               .Join(queryanio,
                      asesoria9 => asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.IdAnio,
                      anio1 => anio1.Id,
                      (asesoria9, anio1) => new { asesoria9, anio1 })
                .Select(x => new
                {
                    Id = x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.Id,
                    nroresolucion = x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.nroresolucion,
                    nombreasesor = x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.usuario1.FullName,
                    fechaini = DateTime.Parse(x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.fechaini.ToString()).ToString("dd/MM/yyyy"),
                    fechafin = DateTime.Parse(x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.fechafin.ToString()).ToString("dd/MM/yyyy"),
                    nombrecarrera = x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.carrera1.nombre,
                    nombrealumno = x.asesoria9.asesoria8.asesoria7.asesoria6.usuario2.FullName,
                    nombrealumno2 = x.asesoria9.usuario3.FullName,
                    estado = x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.asesoria3.asesoria2.asesoria1.activo,
                    nombretipoinvestigacion = x.asesoria9.asesoria8.asesoria7.tipo1.nombre,
                    anio = x.anio1.anio,
                    iduser = x.asesoria9.asesoria8.asesoria7.asesoria6.asesoria5.asesoria4.usuario1.Id,
                })
                .Where(x=>x.iduser== Guid.Parse(user.Id) || Guid.Parse(user.Id) == Guid.Parse("3f8d18e6-d79d-4d1b-938a-0741c3dbcfa4"))
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
