using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels
{
    public class TokenViewModel
    {
        public string token { get; set; }
        public string expiration { get; set; }
    }
}
