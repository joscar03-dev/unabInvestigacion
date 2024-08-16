using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class PostulantExternalMember : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PaternalSurname { get; set; }
        public string MaternalSurname { get; set; }
        public string Dni { get; set; }
        public string Description { get; set; } //Descripción
        public string InstitutionOrigin { get; set; } //Institución de procedencia
        public string Profession { get; set; } //Profesión
        public string CvFilePath { get; set; } //CV
        public string Objectives { get; set; } //Objetivos
        public string Address { get; set; } //Dirección
        public string PhoneNumber { get; set; } //Celular
        public Guid InvestigationConvocationPostulantId { get; set; }
        public InvestigationConvocationPostulant InvestigationConvocationPostulant { get; set; }
    }
}
