using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PublishedBook : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Libro publicado
        public Guid Id { get; set; }
        [Required]
        public string UserId { get; set; } //El congreso le pertenece a un usuario
        public ApplicationUser User { get; set; }
        public string MainAuthor { get; set; } //Autor principal
        public string Title { get; set; } //Titulo del libro
        public string PublishingCity { get; set; } //Ciudad de Edición
        public string PublishingHouse { get; set; } //Editorial
        public int PublishingYear { get; set; } //Año de Edición
        public int PagesCount { get; set; } //Número de Páginas
        public string ISBN { get; set; } //ISBN
        public string LegalDeposit { get; set; } //Depósito legal
        public string Url { get; set; }

        public ICollection<PublishedBookAuthor> PublishedBookAuthors { get; set; }
        public ICollection<PublishedBookFile> PublishedBookFiles { get; set; }
    }
}
