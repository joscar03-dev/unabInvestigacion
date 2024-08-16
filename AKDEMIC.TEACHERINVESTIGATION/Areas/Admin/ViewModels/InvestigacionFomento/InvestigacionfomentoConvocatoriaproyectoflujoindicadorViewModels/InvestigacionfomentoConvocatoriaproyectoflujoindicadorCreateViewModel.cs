using AKDEMIC.DOMAIN.Entities.InvestigacionFomento;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoflujoindicadorViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoflujoindicadorCreateViewModel
    {

        public Guid Id { get; set; }
        public Guid IdConvocatoriaproyecto { get; set; }
        public Guid[] IdConvocatoriaproyectos { get; set; }
        public Guid IdArea { get; set; }
        public Guid[] IdAreas { get; set; }
        public Guid IdListaverificacion { get; set; }
        public Guid[] IdListaverificaciones { get; set; }
        public Guid IdIndicador { get; set; }
        public Guid[] IdIndicadores { get; set; }
        public string valor { get; set; }
        public string[] valores { get; set; }
        public int puntaje { get; set; }

        public string Retornadocente { get; set; }
        public string[] Retornadocentes { get; set; }
        public string[] puntajes { get; set; }
        public string observacion { get; set; }
        public string[] observaciones { get; set; }
        public string estado { get; set; }
        public Guid IdProyectoflujo { get; set; }

        public Guid[] IdProyectoflujos { get; set; }

        
    }
    public class InvestigacionfomentoConvocatoriaproyectoflujoindicadorListValores
    {
        public string valor { get; set; }
    }
}
