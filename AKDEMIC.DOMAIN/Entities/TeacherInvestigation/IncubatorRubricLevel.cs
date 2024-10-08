﻿using AKDEMIC.DOMAIN.Base.Implementations;
using AKDEMIC.DOMAIN.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherInvestigation
{
    public class IncubatorRubricLevel : BaseEntity, ITimestamp, IAggregateRoot
    {
        public Guid Id { get; set; }
        [Column(TypeName = "decimal(18, 4)")]
        public decimal Score { get; set; }
        public string Description { get; set; }

        public Guid IncubatorRubricCriterionId { get; set; }
        public IncubatorRubricCriterion IncubatorRubricCriterion { get; set; }
    }
}
