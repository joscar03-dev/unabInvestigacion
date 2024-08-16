using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherInvestigation.IncubatorConvocationAnnexDatatableSpecifications;
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
    public class IncubatorConvocationAnnexService : IIncubatorConvocationAnnexService
    {
        private readonly IAsyncRepository<IncubatorConvocationAnnex> _incubatorConvocationAnnexRepository;

        public IncubatorConvocationAnnexService(IAsyncRepository<IncubatorConvocationAnnex> incubatorConvocationAnnexRepository)
        {
            _incubatorConvocationAnnexRepository = incubatorConvocationAnnexRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetIncubatorConvocationAnnexDatatable(DataTablesStructs.SentParameters sentParameters, Guid incubatorConvocationId, string searchValue)
        {
            Expression<Func<IncubatorConvocationAnnex, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name); break;
                case "1":
                    orderByPredicate = ((x) => x.Code); break;
            }

            var datatableSpecificationData = new IncubatorConvocationAnnexDatatableSpecification(sentParameters, incubatorConvocationId, orderByPredicate, searchValue);
            var datatableSpecificationCount = new IncubatorConvocationAnnexFilterSpecification(incubatorConvocationId, searchValue);

            var data = await _incubatorConvocationAnnexRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _incubatorConvocationAnnexRepository.CountAsync(datatableSpecificationCount);
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
