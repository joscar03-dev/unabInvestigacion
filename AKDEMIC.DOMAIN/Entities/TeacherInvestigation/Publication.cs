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
    public class Publication : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Required]
        public int WorkCategory { get; set; } //Categoría de trabajo AKDEMIC.CORE.Constants.Systems.TeacherInvestigationConstants.Publication.WorkCategory
        [Required]
        public Guid OpusTypeId { get; set; } //Tipo de Trabajo/obra
        public Guid? PublicationFunctionId { get; set; } //Función
        public Guid? IndexPlaceId { get; set; } //Indexado en
        public Guid? IdentificationTypeId { get; set; } //Identificación
        public Guid? AuthorshipOrderId { get; set; } //Orden Autoria
        [Required]
        public string UserId { get; set; } //La publicacion le pertenece a un usuario
        [Required]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Journal { get; set; } //Revista
        public string Description { get; set; }
        public string Volume { get; set; } //Volumen
        public string Fascicle { get; set; } //Fascículo
        public string MainAuthor { get; set; } //Autor principal
        public string PublishingHouse { get; set; } //Editorial
        public string DOI { get; set; } //Digital Object Identifier
        public string Url { get; set; }


        public Guid? CountryId { get; set; } //Informacion del Api
        public string CountryText { get; set; }

        [Required]
        public DateTime PublishDate { get; set; } //Fecha de Publicación
        public OpusType OpusType { get; set; }
        public PublicationFunction PublicationFunction { get; set; }
        public IndexPlace IndexPlace { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public AuthorshipOrder AuthorshipOrder { get; set; }
        public ApplicationUser User { get; set; }
        public ICollection<PublicationAuthor> PublicationAuthors { get; set; }
        public ICollection<PublicationFile> PublicationFiles { get; set; }
    }
}
