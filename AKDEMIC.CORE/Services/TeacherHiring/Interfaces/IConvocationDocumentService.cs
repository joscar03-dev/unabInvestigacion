using AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationDocument;
using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationDocumentService
    {
        Task<List<DocumentDto>> GetDocuments(Guid convocationId, byte? type = null);
        Task<DataTablesStructs.ReturnedData<object>> GetDatatable(DataTablesStructs.SentParameters parameters, Guid convocationId, byte? type);
    }
}
