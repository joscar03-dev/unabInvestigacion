using System;
using System.Threading.Tasks;
using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services;
using AKDEMIC.CORE.Services.TeacherInvestigation;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.INFRASTRUCTURE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.Pages.InvestigationConvocationPage
{    
    [Authorize(Roles = GeneralConstants.ROLES.SUPERADMIN + "," +
        GeneralConstants.ROLES.TEACHERINVESTIGATION_ADMIN + "," +
        GeneralConstants.ROLES.RESEARCH_PROMOTION_UNIT + "," +
        GeneralConstants.ROLES.INNOVATION_TECHNOLOGY_TRANSFER_UNIT)]
    public class IndexModel : PageModel
    {
        private readonly IDataTablesService _dataTablesService;
        private readonly IInvestigationConvocationService _investigationConvocationService;
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;

        public IndexModel(
            IDataTablesService dataTablesService,
            IInvestigationConvocationService investigationConvocationService,
            IAsyncRepository<InvestigationConvocation> investigationConvocationRepository
            )
        {
            _dataTablesService = dataTablesService;
            _investigationConvocationService = investigationConvocationService;
            _investigationConvocationRepository = investigationConvocationRepository;
        }

        public void OnGet()
        {

        }
        /// <summary>
        /// Esta funcion obtiene una tabla de enumerada por orden de creacion con la data de Convocatorias.
        /// </summary>
        /// <param name="searchValue"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetDatatableAsync(string searchValue)
        {
            var sentParameters = _dataTablesService.GetSentParameters();
            var result = await _investigationConvocationService.GetInvestigationConvocationDatatable(sentParameters, searchValue);

            return new OkObjectResult(result);
        }
        /// <summary>
        /// Esta funcion obtiene la convocatoria por medio de id y lo elimina por medio de una comparacion de id obtenido de la convocatoria con la que esta en la base de datos.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> OnGetInvestigationConvocationDeleteAsync(Guid id)
        {
            var Iconvocation = await _investigationConvocationRepository.GetByIdAsync(id);
            await _investigationConvocationRepository.DeleteAsync(Iconvocation);

            return new OkResult();
        }
    }
}
