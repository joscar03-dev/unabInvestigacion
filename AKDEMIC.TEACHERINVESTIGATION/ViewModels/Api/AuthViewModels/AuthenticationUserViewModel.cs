using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.ViewModels.Api.AuthViewModels
{
    public class AuthenticationUserViewModel
    {
        public string authenticationUserId   {get;set;}
        public string paternalSurname      {get;set;}
        public string maternalSurname      {get;set;}
        public string name                 {get;set;}
        public string userName             {get;set;}
        public string normalizedUserName   {get;set;}
        public string passwordHash         {get;set;}
        public string email                {get;set;}
        public string dni                  {get;set;}
        public string address              { get; set; }
        public List<string> roles          { get; set; }
    }
}
