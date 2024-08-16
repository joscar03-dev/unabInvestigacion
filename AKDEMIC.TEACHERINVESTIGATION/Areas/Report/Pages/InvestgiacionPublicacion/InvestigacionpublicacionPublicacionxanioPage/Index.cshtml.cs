using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Constants.Systems;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MySqlConnector;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Report.Pages.InvestgiacionPublicacion.InvestigacionpublicacionPublicacionxanioPage
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
      /*  public DbSet<SomeModel> SomeModels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SomeModel>().HasNoKey();
        }*/
        public async Task<IActionResult> OnGetDatatableAsync(string anio="2023")
        {

            Console.WriteLine(anio);
            var parm1 = new MySqlParameter("_anio", anio);
              var data2 = _context.InvestigacionpublicacionPublicacionxanios.FromSqlRaw("call sp_investigacionpublicacion_publicacionesxanio(@_anio)", parm1).ToList();

            var result2 = new DataTablesStructs.ReturnedData<object>
            {
                Data = data2,
                DrawCounter = 1,
                RecordsFiltered = 1,
                RecordsTotal = data2.Count(),
            };

            

            return new OkObjectResult(result2);

        }

        public void OnGet()
        {
        }
    }
}
