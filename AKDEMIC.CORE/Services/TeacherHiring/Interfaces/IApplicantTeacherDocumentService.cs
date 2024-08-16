﻿using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IApplicantTeacherDocumentService
    {
        Task<IReadOnlyList<ApplicantTeacherDocument>> GetDocuments(Guid applicantTeacherId);
    }
}
