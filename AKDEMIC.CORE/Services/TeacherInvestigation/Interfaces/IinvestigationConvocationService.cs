using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces
{
    public interface IInvestigationConvocationService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInvestigationConvocationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue = null);

    }
}
