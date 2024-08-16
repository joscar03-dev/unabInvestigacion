using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.ProfileViewModels
{
    public class UserViewModel
    {
        public string Picture { get; set; }
        public string Dni { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CTEVitaeUrl { get; set; }


    }
    public class UserPasswordViewModel
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string RepeatPassword { get; set; }
    }

    public class UserInformationViewModel
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string CTEVitaeUrl { get; set; }
    }
}
