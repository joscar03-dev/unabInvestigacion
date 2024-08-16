using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.UserRequestViewModels
{
    public class UserRequestViewModel
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        public string MaternalSurname { get; set; }

        [Required]
        [MaxLength(250)]
        public string PaternalSurname { get; set; }

        [Required]
        [MaxLength(8)]
        public string Dni { get; set; }

        [Required]
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Type { get; set; } //AKDEMIC.CORE.Constants.GeneralConstants.USERREQUEST_TYPE
    }
}
