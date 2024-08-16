using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.UserViewModels
{
    public class UserEditViewModel
    {
        public string UserId { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Apellido Materno", Prompt = "Apellido Materno")]
        public string MaternalSurname { get; set; }

        [Required]
        [MaxLength(250)]
        [Display(Name = "Apellido Paterno", Prompt = "Apellido Paterno")]
        public string PaternalSurname { get; set; }

        [Display(Name = "Imagen", Prompt = "Imagen")]
        public string Picture { get; set; }

        [Display(Name = "Imagen", Prompt = "Imagen")]
        public IFormFile PictureFile { get; set; }

        [Display(Name = "Teléfono", Prompt = "Teléfono")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Dirección", Prompt = "Dirección")]
        public string Address { get; set; }

        [Display(Name = "Fecha de Nacimiento", Prompt = "Fecha de Nacimiento")]
        public string BirthDate { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [MaxLength(25)]
        [Display(Name = "Usuario", Prompt = "Usuario")]
        public string UserName { get; set; }

        [MaxLength(8)]
        [Display(Name = "DNI", Prompt = "DNI")]
        public string Dni { get; set; }
        public List<string> RolesId { get; set; }
    }
}
