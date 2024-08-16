using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationSectionService
    {
        Task<bool> AnyByTitle(string title, Guid convocationId ,Guid? ignoredId = null);
        Task<IReadOnlyList<ConvocationSection>> GetAllByConvocationId(Guid convocationId);
    }
}
