using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherInterviewSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ApplicantTeacherInterviewService : IApplicantTeacherInterviewService
    {
        private readonly IAsyncRepository<ApplicantTeacherInterview> _repository;

        public ApplicantTeacherInterviewService(
            IAsyncRepository<ApplicantTeacherInterview> repository
            )
        {
            _repository = repository;
        }

        public async Task<ApplicantTeacherInterview> GetInterview(Guid applicantTeacherId, byte type)
        {
            var specification = new ApplicantTeacherInterviewFilterSpecification(applicantTeacherId, type);
            var data = await _repository.FirstOrDefaultAsync(specification);
            return data;
        }
    }
}
