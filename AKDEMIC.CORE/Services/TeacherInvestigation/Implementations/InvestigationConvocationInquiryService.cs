using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherInvestigation.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherInvestigation.InvestigationConvocationInquirySpecifications;
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
    public class InvestigationConvocationInquiryService : IInvestigationConvocationInquiryService
    {
        private readonly IAsyncRepository<InvestigationConvocationInquiry> _investigationConvocationInquiryRepository;

        public InvestigationConvocationInquiryService(IAsyncRepository<InvestigationConvocationInquiry> investigationConvocationInquiryRepository)
        {
            _investigationConvocationInquiryRepository = investigationConvocationInquiryRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetInvestigationConvocationInquiryDatatable(DataTablesStructs.SentParameters sentParameters, Guid investigationConvocationId, string searchValue = null)
        {
            Expression<Func<InvestigationConvocationInquiry, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Inquiry); break;
                case "1":
                    orderByPredicate = ((x) => x.InvestigationConvocationPostulant.User.UserName); break;
                case "2":
                    orderByPredicate = ((x) => x.InvestigationConvocationPostulant.User.FullName); break;
            }

            var datatableSpecificationData = new InvestigationConvocationInquiryDatatableSpecification(sentParameters, investigationConvocationId, orderByPredicate, searchValue);
            var datatableSpecificationCount = new InvestigationConvocationInquiryFilterSpecification(investigationConvocationId, searchValue);

            var data = await _investigationConvocationInquiryRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _investigationConvocationInquiryRepository.CountAsync(datatableSpecificationCount);
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
