using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherDocumentSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ApplicantTeacherDocumentService : IApplicantTeacherDocumentService
    {
        private readonly IAsyncRepository<ApplicantTeacherDocument> _applicantTeacherDocumentRepository;

        public ApplicantTeacherDocumentService(
            IAsyncRepository<ApplicantTeacherDocument> applicantTeacherDocumentRepository
            )
        {
            _applicantTeacherDocumentRepository = applicantTeacherDocumentRepository;
        }

        public async Task<IReadOnlyList<ApplicantTeacherDocument>> GetDocuments(Guid applicantTeacherId)
        {
            var documentSpecification = new ApplicantTeacherDocumentFilterSpecification(applicantTeacherId);
            var data = await _applicantTeacherDocumentRepository.ListAsync(documentSpecification);
            return data;
        }
    }
}
