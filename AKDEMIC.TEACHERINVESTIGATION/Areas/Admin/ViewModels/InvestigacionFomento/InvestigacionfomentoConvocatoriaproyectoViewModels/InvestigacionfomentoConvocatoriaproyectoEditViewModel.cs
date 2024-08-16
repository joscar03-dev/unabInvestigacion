using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdConvocatoria { get; set; }
        public Guid IdLinea { get; set; }

        public string nombre { get; set; }
        public string archivourlcarta { get; set; }

        public IFormFile File { get; set; }

        public string objetivoprincipal { get; set; }

        public decimal presupuesto { get; set; }
        public Guid IdDocente { get; set; }
        public string estado { get; set; }
        public string retornadocente { get; set; }

        public int nromeses { get; set; }
        public string tipo { get; set; }
        public string problema { get; set; }
        public string antecedente { get; set; }
        public string resultado { get; set; }
        public string justificacion { get; set; }
        public string hipotesis { get; set; }
        public string preguntas { get; set; }
        public string objetivogeneral { get; set; }
        public string objetivoespecifico { get; set; }
        public string metodologia { get; set; }
        public string riesgos { get; set; }
        public string resumencientifico { get; set; }
        public string equipamiento { get; set; }
        public string resultados { get; set; }
        public string sostenibilidad { get; set; }
        public string impacto { get; set; }
        public string descripcionemprendimiento { get; set; }
        public string ventajascompetitivascomparativas { get; set; }
        public string estudiomercado { get; set; }
        public string estrategiamarketing { get; set; }
        public string potencialessociosestrategicos { get; set; }

        
    }
}
