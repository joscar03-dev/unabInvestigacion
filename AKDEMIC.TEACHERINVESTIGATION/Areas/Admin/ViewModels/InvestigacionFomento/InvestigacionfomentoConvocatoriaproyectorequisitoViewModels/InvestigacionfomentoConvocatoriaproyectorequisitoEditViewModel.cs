using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectorequisitoViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectorequisitoEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoria { get; set; }
        public Guid IdLinea { get; set; }

        public string nombre { get; set; }
        public string archivourlcarta { get; set; }

        public IFormFile File { get; set; }

        public string objetivoprincipal { get; set; }

        public decimal presupuesto { get; set; }

        public string estado { get; set; }

        public List<InvestigacionfomentoMaestroAreaViewModal> MaestroArea { get; set; }

    }
    public class InvestigacionfomentoMaestroAreaViewModal
    {
        public Guid Id { get; set; }

        public Guid IdUser { get; set; }

        public string nombre { get; set; }




    }
    public class InvestigacionfomentoListaindicadoresnew
    {
        public Guid Id { get; set; }





    }
}
