using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;




namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.InvestigacionfomentoConvocatoriaproyectoflujoindicadorViewModels
{
    public class InvestigacionfomentoConvocatoriaproyectoflujoindicadorEditViewModel
    {
        public Guid Id { get; set; }
        public Guid IdProyectoflujo { get; set; }
        public Guid IdListaverificacion { get; set; }
        public Guid IdIndicador { get; set; }
        public string valor { get; set; }

        public int puntaje { get; set; }
        public string observacion { get; set; }

        public List<InvestigacionfomentoMaestroAreaViewModal> MaestroArea { get; set; }

    }
    public class InvestigacionfomentoMaestroAreaViewModal
    {
        public Guid Id { get; set; }

        public Guid IdUser { get; set; }

        public string nombre { get; set; }




    }
   
}
