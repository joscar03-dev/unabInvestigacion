using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.InvestigacionFomento.MaestroAreasusuarioViewModels
{
    public class MaestroAreasusuarioCreateViewModel
    {
      
        [Required]
        public Guid Id { get; set; }
        [Required]
        public Guid IdArea { get; set; }
        public Guid IdUser { get; set; }
        public string activo { get; set; }


    }
}
