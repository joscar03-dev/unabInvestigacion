using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationQuestionService
    {
        Task<IReadOnlyList<ConvocationQuestion>> GetAllBySectionId(Guid convocationSectionId);
        Task<IReadOnlyList<ConvocationQuestion>> GetAllBySectionsId(List<Guid> convocationSectionId);
        Task<bool> AnyByDescription(Guid convocationSectionId, string description, Guid? ignoredId = null);
    }
}
