using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IApplicantTeacherRubricSectionDocumentService
    {
        Task<IReadOnlyList<ApplicantTeacherRubricSectionDocument>> GetRubricSectionDocuments(Guid applicantTeacherId);
    }
}
