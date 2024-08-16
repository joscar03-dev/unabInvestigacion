using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.IncubatorUnit.ViewModels.IncubatorConvocationViewModels
{
    public class IncubatorRubricImportViewModel
    {
        public Guid IncubatorConvocationId { get; set; } //Identificador de Convocatoria Actual
        public string ConvocationCodeToExport { get; set; } //Código de la Convocatoria del cual copiaremos su rubrica
    }
}
