using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.CORE.Services
{
    public interface IDataTablesService
    {
        int GetDrawCounter();
        string GetOrderColumn();
        string GetOrderDirection();
        int GetPagingFirstRecord();
        int GetRecordsPerDraw();
        string GetSearchValue();
        SentParameters GetSentParameters();
        object GetPaginationObject<T>(int recordsFiltered, IEnumerable<T> data);
    }
}
