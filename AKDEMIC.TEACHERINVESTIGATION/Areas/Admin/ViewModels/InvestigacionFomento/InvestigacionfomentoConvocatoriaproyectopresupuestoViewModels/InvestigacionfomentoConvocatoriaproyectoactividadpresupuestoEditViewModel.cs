using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectopresupuestoViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoactividadpresupuestoEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }
        public Guid IdUnidadmedida { get; set; }
        public Guid IdConcepto { get; set; }
        public Guid IdGastotipo { get; set; }

        public string descripcion { get; set; }

        public decimal costounitario { get; set; }
        public decimal cantidad { get; set; }
        public decimal total { get; set; }


    }
}
