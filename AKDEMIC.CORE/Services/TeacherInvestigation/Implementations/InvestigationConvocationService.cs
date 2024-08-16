using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherInvestigation;
using AKDEMIC.CORE.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;

namespace AKDEMIC.CORE.Services.TeacherInvestigation.Implementations
{
    public class InvestigationConvocationService : IInvestigationConvocationService
    {
        private readonly IAsyncRepository<InvestigationConvocation> _investigationConvocationRepository;

        public InvestigationConvocationService(IAsyncRepository<InvestigationConvocation> investigationConvocationRepository)
        {
            _investigationConvocationRepository = investigationConvocationRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInvestigationConvocationDatatable(DataTablesStructs.SentParameters sentParameters,string searchValue = null)
        {
            Expression<Func<InvestigationConvocation, dynamic>> orderByPredicate = null;
            switch(sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Name);
                    break;
            }

            var datatableSpecificationData = new InvestigationConvocationDatatableSpecification(sentParameters, orderByPredicate, searchValue);
            var datatableSpecificationCount = new InvestigationConvocationFilterSpecification(searchValue);

            var data = await _investigationConvocationRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _investigationConvocationRepository.CountAsync(datatableSpecificationCount);
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
