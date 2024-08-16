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
    public class PublishedChapterBook : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Capitulo de Libro publicado
        public Guid Id { get; set; }
        [Required]
        public string UserId { get; set; } //El congreso le pertenece a un usuario
        public ApplicationUser User { get; set; }
        public string MainAuthor { get; set; } //Autor principal
        public string BookTitle { get; set; } //Título del libro
        public string ChapterTitle { get; set; } //Título del capítulo
        public string PublishingCity { get; set; } //Ciudad de Edición
        public string PublishingHouse { get; set; } //Editorial
        public int PublishingYear { get; set; } //Año de Edición
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public string DOI { get; set; } //Digital Object Identifier
        public string ISBN { get; set; } //ISBN
        public string Url { get; set; }

        public ICollection<PublishedChapterBookAuthor> PublishedChapterBookAuthors { get; set; }
        public ICollection<PublishedChapterBookFile> PublishedChapterBookFiles { get; set; }
    }
}
