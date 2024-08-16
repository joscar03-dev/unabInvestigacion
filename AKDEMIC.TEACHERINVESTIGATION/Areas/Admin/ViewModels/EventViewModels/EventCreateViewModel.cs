using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.EventViewModels
{
    public class EventCreateViewModel
    {

        public string Title { get; set; }

        public string Description { get; set; }
     
        public string EventDate { get; set; }
      
        public IFormFile PictureFile { get; set; }
    
        public string VideoUrl { get; set; }
 
        public decimal Cost { get; set; }
   
        public string Organizer { get; set; }

        public Guid? UnitId { get; set; }
    }
}
