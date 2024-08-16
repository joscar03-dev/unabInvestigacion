using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConvocationDatatable(DataTablesStructs.SentParameters sentParameters, string searchvalue, ClaimsPrincipal user = null);
        Task<Select2Structs.ResponseParameters> GetSelect2(Select2Structs.RequestParameters requestParameters);
    }
}
