using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.General.Interfaces
{
    public interface IApplicationUserService
    {
        Task<bool> UserExistsByUserName(string userName = null);
        Task<Select2Structs.ResponseParameters> GetSelect2(Select2Structs.RequestParameters requestParameters, List<string> ignoredRoles = null, List<string> selectedRoles = null);
        Task<DataTablesStructs.ReturnedData<object>> GetUsersDatatable(DataTablesStructs.SentParameters parameters, List<string> roles, string search);
    }
}
