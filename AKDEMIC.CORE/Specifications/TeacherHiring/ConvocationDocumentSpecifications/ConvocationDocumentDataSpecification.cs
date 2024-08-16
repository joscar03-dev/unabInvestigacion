using AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationDocument;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using Ardalis.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationDocumentSpecifications
{
    public sealed class ConvocationDocumentDataSpecification : Specification<ConvocationDocument, DocumentDto>
    {
        public ConvocationDocumentDataSpecification(Guid convocationId, byte? type = null)
        {
            Query.Where(x => x.ConvocationId == convocationId);

            if (type.HasValue)
            {
                Query.Where(x => x.Type == type);
            }

            Query
                .Select(x => new DocumentDto
                {
                    ConvocationId = x.ConvocationId,
                    CreatedAt = x.CreatedAt.ToLocalDateTimeFormat(),
                    Id = x.Id,
                    Name = x.Name,
                    Url = x.Url,
                    HasApplicantTeacherDocuments = x.ApplicantTeacherDocuments.Any(),
                    Type = x.Type
                });
        }
    }
}
