using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorSuppliesExpense : BaseEntity, ITimestamp, IAggregateRoot
    {
        //Insumos y Materiales
        public Guid Id { get; set; }
        public string ExpenseCode { get; set; } //	Especifica de gasto
        public string Description { get; set; } //Descripción
        public string MeasureUnit { get; set; } //Unidad de Medida
        public int Quantity { get; set; } //Cantidad
        public decimal UnitPrice { get; set; } //Precio Unitario
        public string ActivityJustification { get; set; } //Actividades en la que se emplea

        public Guid IncubatorPostulationId { get; set; }
        public IncubatorPostulation IncubatorPostulation { get; set; }
    }
}
