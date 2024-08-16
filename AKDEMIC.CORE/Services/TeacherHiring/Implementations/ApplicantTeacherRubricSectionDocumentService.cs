using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherRubricSectionDocumentSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ApplicantTeacherRubricSectionDocumentService : IApplicantTeacherRubricSectionDocumentService
    {
        private readonly IAsyncRepository<ApplicantTeacherRubricSectionDocument> _repository;

        public ApplicantTeacherRubricSectionDocumentService(
            IAsyncRepository<ApplicantTeacherRubricSectionDocument> repository
            )
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<ApplicantTeacherRubricSectionDocument>> GetRubricSectionDocuments(Guid applicantTeacherId)
        {
            var documentSpecification = new ApplicantTeacherRubricSectionDocumentFilterSpecification(applicantTeacherId);
            var data = await _repository.ListAsync(documentSpecification);
            return data;

        }
    }
}
