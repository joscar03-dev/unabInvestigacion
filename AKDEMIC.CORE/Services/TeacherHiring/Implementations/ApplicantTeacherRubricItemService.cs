using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherRubricItemSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ApplicantTeacherRubricItemService : IApplicantTeacherRubricItemService
    {
        private readonly IAsyncRepository<ApplicantTeacherRubricItem> _repository;

        public ApplicantTeacherRubricItemService(
            IAsyncRepository<ApplicantTeacherRubricItem> repository
            )
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<ApplicantTeacherRubricItem>> GetRubricItems(Guid applicantTeacherId, byte sectionType, string evaluatorId = null)
        {
            var itemSpecifications = new ApplicantTeacherRubricItemFilterSpecification(applicantTeacherId, sectionType, evaluatorId);
            var data = await _repository.ListAsync(itemSpecifications);
            return data;
        }
    }
}
