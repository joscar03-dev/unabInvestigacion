using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaViewModels
{
    public class InvestigacionfomentoConvocatoriaCreateViewModel
    {
        public Guid Id { get; set; }
        public Guid IdCategoriaconvocatoria { get; set; }
        public Guid IdTipoconvocatoria { get; set; }
        public Guid IdFlujo { get; set; }

        public Guid IdFacultad { get; set; }

        public string fechaini { get; set; }
        public string fechafin { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string activo { get; set; }
        public IFormFile PictureFile { get; set; }
        public string fechainiinscripcion { get; set; }

        public string fechafininscripcion { get; set; }

        public string dirigidoa { get; set; }

        public string imagenarchivo { get; set; }
        public int nroplaza { get; set; }

        public decimal presupuesto { get; set; }
    }
}
