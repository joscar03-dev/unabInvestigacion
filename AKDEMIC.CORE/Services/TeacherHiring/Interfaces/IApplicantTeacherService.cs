using AKDEMIC.CORE.Structs;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IApplicantTeacherService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetApplicantTeachersDatatable(DataTablesStructs.SentParameters parameters, Guid? convocationId, string search, ClaimsPrincipal user = null);
        Task<bool> AnyByConvocation(Guid convocationId);
        Task<int> GetPostulantsCountByConvocationVacancy(Guid convocationVacancyId);
    }
}
