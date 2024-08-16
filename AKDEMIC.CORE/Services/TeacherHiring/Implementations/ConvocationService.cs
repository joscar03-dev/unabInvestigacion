using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationSpecifications;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationService : IConvocationService
    {
        private readonly IAsyncRepository<Convocation> _convocationRepository;

        public ConvocationService(IAsyncRepository<Convocation> convocationRepository)
        {
            _convocationRepository = convocationRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConvocationDatatable(DataTablesStructs.SentParameters sentParameters, string searchValue, ClaimsPrincipal user = null)
        {
            Expression<Func<Convocation, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "1":
                    orderByPredicate = ((x) => x.Name); break;
                case "2":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "3":
                    orderByPredicate = ((x) => x.EndDate); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var datatableSpecificationData = new ConvocationDatatableSpecification(sentParameters, orderByPredicate, searchValue, user);
            var datatableSpecificationCount = new ConvocationFilterSpecification(searchValue, user);

            var data = await _convocationRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _convocationRepository.CountAsync(datatableSpecificationCount);
            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }
    
        public async Task<Select2Structs.ResponseParameters> GetSelect2(Select2Structs.RequestParameters requestParameters)
        {
            var selectSpecification = new ConvocationSelectSpecification(requestParameters);
            var data = await _convocationRepository.ListAsync(selectSpecification);

            var result = new Select2Structs.ResponseParameters
            {
                Pagination = new Select2Structs.Pagination
                {
                    More = data.Count >= GeneralConstants.SELECT2.DEFAULT.PAGE_SIZE
                },
                Results = data
            };

            return result;
        }
    }
}
