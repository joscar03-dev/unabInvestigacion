using System;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.EventViewModels
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
        public string ImageQR { get; set; }
        public string UserSigned { get; set; }
        public bool SignedUp { get; set; }
    }

    public class EventGuestParticipantViewModel
    {
        public Guid EventId { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Name { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string University { get; set; }
        public string BirthDate { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class EventUserParticipantViewModel
    {
        public Guid EventId { get; set; }
    }
}
