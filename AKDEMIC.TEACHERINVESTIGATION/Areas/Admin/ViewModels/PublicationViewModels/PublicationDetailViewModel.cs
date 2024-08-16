using System;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Admin.ViewModels.PublicationViewModels
{
    public class PublicationDetailViewModel
    {
        public Guid? Id { get; set; }
        public string UserFullName { get; set; }
        public string UserName { get; set; }

        public string AuthorShipOrderName { get; set; }

        public string OpusTypeName { get; set; }
        public string PublicationFunctionName { get; set; }

        public string IndexPlaceName { get; set; }

        public string IdentificationTypeName { get; set; }
        public int WorkCategory { get; set; }
        public string WorkCategoryName { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Journal { get; set; } //Revista
        public string Description { get; set; }
        public string Volume { get; set; } //Volumen
        public string Fascicle { get; set; } //Fascículo
        public string MainAuthor { get; set; } //Autor principal
        public string PublishingHouse { get; set; } //Editorial
        public string DOI { get; set; } //Digital Object Identifier
        public string PublishDate { get; set; }
    }   
}
