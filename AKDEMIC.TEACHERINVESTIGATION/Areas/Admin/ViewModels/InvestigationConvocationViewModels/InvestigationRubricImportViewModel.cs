using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigationConvocationViewModels
{
    public class InvestigationRubricImportViewModel
    {
        public Guid InvestigationConvocationId { get; set; } //Identificador de Convocatoria Actual
        public string ConvocationCodeToExport { get; set; } //Código de la Convocatoria del cual copiaremos su rubrica
    }
}
