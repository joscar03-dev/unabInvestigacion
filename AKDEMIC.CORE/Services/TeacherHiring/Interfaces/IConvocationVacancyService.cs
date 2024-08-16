using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationVacancyService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConvocationVacanciesDatatable(DataTablesStructs.SentParameters sentParameters, Guid convocationId, Guid? academicDepartmentId);
        Task<IReadOnlyList<ConvocationVacancy>> GetAllByConvocationId(Guid convocationId);
    }
}
