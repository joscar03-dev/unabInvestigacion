using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces
{
    public interface IIncubatorConvocationAnnexService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetIncubatorConvocationAnnexDatatable(DataTablesStructs.SentParameters sentParameters, Guid incubatorConvocationId, string searchValue = null);
    }
}
