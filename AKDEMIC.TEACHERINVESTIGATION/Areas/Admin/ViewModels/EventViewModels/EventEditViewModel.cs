using Microsoft.AspNetCore.Http;
using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.EventViewModels
{
    public class EventEditViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public string Description { get; set; }

        public string EventDate { get; set; }

        public IFormFile PictureFile { get; set; }

        public string VideoUrl { get; set; }

        public decimal Cost { get; set; }

        public string Organizer { get; set; }

        public Guid? UnitId { get; set; }

        public string PicturePath { get; set; }
    }
}
