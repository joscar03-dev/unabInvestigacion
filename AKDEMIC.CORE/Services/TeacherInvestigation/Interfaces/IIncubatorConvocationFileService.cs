using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces
{
    public interface IIncubatorConvocationFileService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetIncubatorConvocationFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid incubatorConvocationId, string searchValue = null);
    }
}
