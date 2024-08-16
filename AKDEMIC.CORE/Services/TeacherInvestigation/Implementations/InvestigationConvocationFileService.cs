using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationFileSpecifications;
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
    public class InvestigationConvocationFileService : IInvestigationConvocationFileService
    {
        private readonly IAsyncRepository<InvestigationConvocationFile> _investigationConvocationFileRepository;

        public InvestigationConvocationFileService(IAsyncRepository<InvestigationConvocationFile> investigationConvocationFileRepository)
        {
            _investigationConvocationFileRepository = investigationConvocationFileRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInvestigationConvocationFileDatatable(DataTablesStructs.SentParameters sentParameters, Guid investigationConvocationId, string searchValue)
        {
            Expression<Func<InvestigationConvocationFile, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Number); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
            }

            var datatableSpecificationData = new InvestigationConvocationFileDatatableSpecification(sentParameters, investigationConvocationId, orderByPredicate, searchValue);
            var datatableSpecificationCount = new InvestigationConvocationFileFilterSpecification(investigationConvocationId , searchValue);

            var data = await _investigationConvocationFileRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _investigationConvocationFileRepository.CountAsync(datatableSpecificationCount);
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
