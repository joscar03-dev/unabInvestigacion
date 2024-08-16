using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces
{
    public interface IInvestigationConvocationFileService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetInvestigationConvocationFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid investigationConvocationId, string searchValue = null);
    }
}
