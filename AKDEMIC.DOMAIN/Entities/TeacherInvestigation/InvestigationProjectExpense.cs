using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class InvestigationProjectExpense : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Gastos
        public Guid Id { get; set; }
        public Guid InvestigationProjectTaskId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ProductType { get; set; } //Tipo de producto
        public string ExpenseCode { get; set; } //	Especifica de gasto
        public InvestigationProjectTask InvestigationProjectTask { get; set; }
    }
}
