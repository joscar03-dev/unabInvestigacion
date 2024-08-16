using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherInvestigation.IncubatorConvocationFileDatatableSpecifications;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherInvestigation.Implementations
{
    public class IncubatorConvocationFileService : IIncubatorConvocationFileService
    {
        private readonly IAsyncRepository<IncubatorConvocationFile> _incubatorConvocationFileRepository;

        public IncubatorConvocationFileService(IAsyncRepository<IncubatorConvocationFile> incubatorConvocationFileRepository)
        {
            _incubatorConvocationFileRepository = incubatorConvocationFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubatorConvocationFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid incubatorConvocationId, string searchValue)
        {
            Expression<Func<IncubatorConvocationFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Number); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
            }

            var datatableSpecificationData = new IncubatorConvocationFileDatatableSpecification(sentParameters, incubatorConvocationId, orderByPredicate, searchValue);
            var datatableSpecificationCount = new IncubatorConvocationFileFilterSpecification(incubatorConvocationId, searchValue);

            var data = await _incubatorConvocationFileRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _incubatorConvocationFileRepository.CountAsync(datatableSpecificationCount);
            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    }
}
