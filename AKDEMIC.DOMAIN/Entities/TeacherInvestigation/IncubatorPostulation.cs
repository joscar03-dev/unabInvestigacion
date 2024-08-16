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
    public class IncubatorPostulation : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } //Postulante
        public ApplicationUser User { get; set; }
        public Guid IncubatorConvocationId { get; set; }
        public IncubatorConvocation IncubatorConvocation { get; set; }
        public DateTime RegisterDate { get; set; }
        public int ReviewState { get; set; }


        #region Informacion General

        public string Title { get; set; }//Nombre del Proyecto
        public decimal Budget { get; set; } //Presupuesto
        public int MonthDuration { get; set; } //Duración del Proyecto en Meses
        public Guid? DepartmentId { get; set; } //Informacion del Api
        public string DepartmentText { get; set; }
        public Guid? ProvinceId { get; set; } //Informacion del Api
        public string ProvinceText { get; set; }
        public Guid? DistrictId { get; set; } //Informacion del Api
        public string DistrictText { get; set; }
        public string GeneralGoals { get; set; }//Objetivos Generales
        public ICollection<IncubatorPostulationSpecificGoal> IncubatorPostulationSpecificGoals { get; set; } //Objetivos Especificos

        public string CVFilePath { get; set; } //CV

        #endregion


        #region Equipo de Investigacion

        public string AdviserId { get; set; } //Docente Asesor del Api Teacher.UserId
        public string CoAdviserId { get; set; } //Docente co Asesor del Api Teacher.UserId
        public ICollection<IncubatorPostulationTeamMember> IncubatorPostulationTeamMembers { get; set; } //Estudiantes traidos del api

        #endregion

        #region Idea de Negocio
        public string BusinessIdeaDescription { get; set; } //Descripción de idea de negocio
        public string CompetitiveAdvantages { get; set; } //Ventajas Competitivas
        public string MarketStudy { get; set; } //Estudio de mercado
        public string MarketingPlan { get; set; } //Plan de marketing
        public string Resources { get; set; } //Organización, Recursos Humanos, Económico y Financiero
        public string PotentialStrategicPartners { get; set; } //Potenciales Socios Estrateguicos
        #endregion

        #region Plan de Negocios
        public string Mission { get; set; } //Misión y objetivos estratégicos
        public string ProductDescription { get; set; } //Descripción del producto o servicio
        public string TechnicalViability { get; set; } //Estudio de viabilidad técnica
        public string EconomicViability { get; set; } //Estudio de viabilidad económica y financiera
        public string MerchandisingPlan { get; set; } //Plan de comercialización
        public string Breakeven { get; set; } //Determinación del Punto de Equilibrio
        public string AffectationLevel { get; set; } //Nivel de afectación
        #endregion


        public ICollection<IncubatorPostulationAnnex> IncubatorPostulationAnnexes { get; set; }
        public ICollection<IncubatorEquipmentExpense> IncubatorEquipmentExpenses { get; set; }
        public ICollection<IncubatorSuppliesExpense> IncubatorSuppliesExpenses { get; set; }
        public ICollection<IncubatorThirdPartyServiceExpense> IncubatorThirdPartyServiceExpenses { get; set; }
        public ICollection<IncubatorOtherExpense> IncubatorOtherExpenses { get; set; }
    }
}
