using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.UnitBoss.ViewModels.EventViewModels
{
    public class EventViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Organizer { get; set; }
        public string PicturePath { get; set; }
        public string EventDate { get; set; }
        public string Cost { get; set; }
    }

    public class EventDetailViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Organizer { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string PicturePath { get; set; }
        public string EventDate { get; set; }
        public string Cost { get; set; }
        public string Unit { get; set; }
        public string UserSigned { get; set; }
        public bool SignedUp { get; set; }
    }
}
