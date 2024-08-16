using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.InvestigacionPublicacion
{
    public class InvestigacionpublicacionPublicacionxanio : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public int WorkCategory { get; set; }
        public String PublicationFunctionName { get; set; }
        public String IndexPlaceName { get; set; }
        public String IdentificationTypeName { get; set; }
        public String AuthorshipOrderName { get; set; }
        public DateTime PublishDate { get; set; }
        public String Title { get; set; }

        public String  OpusTypeName { get; set; }
        public String SubTitle { get; set; }
        public String Description { get; set; }
        public String Volume { get; set; }
        public String MainAuthor { get; set; }
        public String DOI { get; set; }
        public String tipocongreso { get; set; }
        public String nombrecongreso { get; set; }
        public String satisfaccion { get; set; }
        public String isntitucion { get; set; }
        public String quartil { get; set; }
        public String autores { get; set; }


    }
}
