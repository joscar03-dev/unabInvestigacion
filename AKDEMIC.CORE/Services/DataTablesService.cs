﻿using AKDEMIC.CORE.Structs;
using AKDEMIC.CORE.Constants;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AKDEMIC.CORE.Structs.DataTablesStructs;

namespace AKDEMIC.CORE.Services
{
    public class DataTablesService : IDataTablesService
    {
        public IHttpContextAccessor _httpContextAccessor;

        public DataTablesService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetDrawCounter()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            int.TryParse(request.Query[GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.DRAW_COUNTER], out int result);
            return result;
        }

        public string GetOrderColumn()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query.ContainsKey(GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_COLUMN) ?
                request.Query[GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_COLUMN].ToString() :
                null;
        }

        public string GetOrderDirection()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query.ContainsKey(GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_DIRECTION) ?
                request.Query[GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.ORDER_DIRECTION].ToString().ToUpper() :
                GeneralConstants.DATATABLE.SERVER_SIDE.DEFAULT.ORDER_DIRECTION;
        }

        public object GetPaginationObject<T>(int recordsFiltered, IEnumerable<T> data)
        {
            return new ReturnedData<T>
            {
                Data = data,
                DrawCounter = GetDrawCounter(),
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsFiltered
            };
        }

        public int GetPagingFirstRecord()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            int.TryParse(request.Query[GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.PAGING_FIRST_RECORD], out int result);
            return result;
        }

        public int GetRecordsPerDraw()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            int.TryParse(request.Query[GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.RECORDS_PER_DRAW], out int result);
            return result;
        }

        public string GetSearchValue()
        {
            var request = _httpContextAccessor.HttpContext.Request;

            return request.Query[GeneralConstants.DATATABLE.SERVER_SIDE.SENT_PARAMETERS.SEARCH_VALUE];
        }

        public DataTablesStructs.SentParameters GetSentParameters()
        {
            return new SentParameters
            {
                DrawCounter = GetDrawCounter(),
                OrderColumn = GetOrderColumn(),
                OrderDirection = GetOrderDirection(),
                PagingFirstRecord = GetPagingFirstRecord(),
                RecordsPerDraw = GetRecordsPerDraw()
            };
        }
    }
}
