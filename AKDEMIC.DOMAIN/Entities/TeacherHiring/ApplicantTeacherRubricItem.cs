using AKDEMIC.DOMAIN.Base.Interfaces;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.DOMAIN.Entities.TeacherHiring
{
    public class ApplicantTeacherRubricItem : IAggregateRoot
    {
        public Guid Id { get; set; }
        public Guid ApplicantTeacherId { get; set; }
        public Guid ConvocationRubricItemId { get; set; }
        public ApplicantTeacher ApplicantTeacher { get; set; }
        public ConvocationRubricItem ConvocationRubricItem { get; set; }
        public decimal Score { get; set; }

        public string EvaluatorId { get; set; }
        public ApplicationUser Evaluator { get; set; }
    }
}
