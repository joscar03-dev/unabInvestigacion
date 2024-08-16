using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Student.ViewModels.IncubatorPostulationViewModels
{
    public class InscriptionViewModel
    {
        public Guid IncubatorPostulationId { get; set; }
    }

    #region EquipmentExpense

    public class EquipmentExpenseAddViewModel : InscriptionViewModel
    {
        [Required]
        public string ExpenseCode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string MeasureUnit { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public string ActivityJustification { get; set; }

    }

    public class EquipmentExpenseEditViewModel : EquipmentExpenseAddViewModel
    {
        public Guid Id { get; set; }
    }

    #endregion

    #region SupplyExpense

    public class SupplyExpenseAddViewModel : InscriptionViewModel
    {
        [Required]
        public string ExpenseCode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string MeasureUnit { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public string ActivityJustification { get; set; }

    }

    public class SupplyExpenseEditViewModel : SupplyExpenseAddViewModel
    {
        public Guid Id { get; set; }
    }

    #endregion

    #region ThirdPartyServiceExpense

    public class ThirdPartyServiceExpenseAddViewModel : InscriptionViewModel
    {
        [Required]
        public string ExpenseCode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string MeasureUnit { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public string ActivityJustification { get; set; }

    }

    public class ThirdPartyServiceExpenseEditViewModel : ThirdPartyServiceExpenseAddViewModel
    {
        public Guid Id { get; set; }
    }

    #endregion

    #region OtherExpense

    public class OtherExpenseAddViewModel : InscriptionViewModel
    {
        [Required]
        public string ExpenseCode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string MeasureUnit { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        public string ActivityJustification { get; set; }

    }

    public class OtherExpenseEditViewModel : OtherExpenseAddViewModel
    {
        public Guid Id { get; set; }
    }

    #endregion

    #region TeamMember

    public class TeamMemberAddViewModel : InscriptionViewModel
    {
        
        public string UserName { get; set; }
        
        public string PaternalSurName { get; set; }
        
        public string MaternalSurName { get; set; }
        
        public string Name { get; set; }
        
        public int Sex { get; set; }
        public string CareerText { get; set; }

    }

    #endregion

    public class InvestigationTeamAddViewModel : InscriptionViewModel
    {
        public string AdviserId { get; set; }
        public string CoAdviserId { get; set; }
    }

    public class SpecificGoalAddViewModel: InscriptionViewModel
    {
        [Required]
        public string Description { get; set; }
        [Required]
        public int OrderNumber { get; set; }
    }

    public class SpecificGoalEditViewModel : SpecificGoalAddViewModel
    {
        public Guid Id { get; set; }
    }

    public class InscriptionGeneralInformationViewModel: InscriptionViewModel
    {
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

        public string CVFilePath { get; set; } //CV

    }

    public class BusinessIdeaViewModel : InscriptionViewModel
    {
        public string BusinessIdeaDescription { get; set; } //Descripción de idea de negocio
        public string CompetitiveAdvantages { get; set; } //Ventajas Competitivas
        public string MarketStudy { get; set; } //Estudio de mercado
        public string MarketingPlan { get; set; } //Plan de marketing
        public string Resources { get; set; } //Organización, Recursos Humanos, Económico y Financiero
        public string PotentialStrategicPartners { get; set; } //Potenciales Socios Estrateguicos

    }

    public class BusinessPlanViewModel : InscriptionViewModel
    {
        public string Mission { get; set; } //Misión y objetivos estratégicos
        public string ProductDescription { get; set; } //Descripción del producto o servicio
        public string TechnicalViability { get; set; } //Estudio de viabilidad técnica
        public string EconomicViability { get; set; } //Estudio de viabilidad económica y financiera
        public string MerchandisingPlan { get; set; } //Plan de comercialización
        public string Breakeven { get; set; } //Determinación del Punto de Equilibrio
        public string AffectationLevel { get; set; } //Nivel de afectación

    }

    public class ScheduleViewModels
    {
        public int MonthDuration { get; set; }
        public List<SpecificGoalViewModel> SpecificGoals { get; set; }
    }

    public class SpecificGoalViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public List<ActivityViewModel> Activities { get; set; }
    }
    public class ActivityViewModel
    {
        public Guid Id { get; set; }
        public Guid IncubatorPostulationSpecificGoalId { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
        public List<ActivityMonthViewModel> ActivityMonths { get; set; }
    }

    public class ActivityMonthViewModel
    {
        public Guid IncubatorPostulationActivityId { get; set; }
        public int MonthNumber { get; set; }
    }

    public class ActivityAddViewModel
    {
        public Guid IncubatorPostulationSpecificGoalId { get; set; }
        public string Description { get; set; }
        public int OrderNumber { get; set; }
    }

    public class ActivityEditViewModel: ActivityAddViewModel
    {
        public Guid Id { get; set; }
    }

    public class ActivityMonthSaveViewModel
    {
        public Guid IncubatorPostulationActivityId { get; set; }
        public List<int> Months { get; set; }
    }
}
