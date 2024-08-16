using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationCalendarService
    {
        Task<DataTablesStructs.ReturnedData<object>> GetConvocationCalendarDatatable(DataTablesStructs.SentParameters sentParameters, Guid convocationId);
    }
}
