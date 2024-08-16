using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.TEACHERINVESTIGATION.Areas.Teacher.ViewModels.InvestigationProjectViewModels
{
    public class InvestigationProjectExpenseViewModel
    {
        public Guid InvestigationProjectId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public Guid InvestigationProjectTaskId { get; set; }
		public string ProductType { get; set; } //Tipo de producto
		public string ExpenseCode { get; set; } //o	Especifica de gasto

	}

    public class InvestigationProjectExpenseEditViewModel
    {
        public Guid Id { get; set; }
        public Guid InvestigationProjectId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
		public Guid InvestigationProjectTaskId { get; set; }
		public string ProductType { get; set; } //Tipo de producto
		public string ExpenseCode { get; set; } //o	Especifica de gasto
	}
}
