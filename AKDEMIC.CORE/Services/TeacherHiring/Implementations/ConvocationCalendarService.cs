using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationCalendarSpecifications;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationCalendarService : IConvocationCalendarService
    {
        private readonly IAsyncRepository<ConvocationCalendar> _convocationCalendarRepository;

        public ConvocationCalendarService(
            IAsyncRepository<ConvocationCalendar> convocationCalendarRepository
            )
        {
            _convocationCalendarRepository = convocationCalendarRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConvocationCalendarDatatable(DataTablesStructs.SentParameters sentParameters, Guid convocationId)
        {
            Expression<Func<ConvocationCalendar, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.Description); break;
                case "1":
                    orderByPredicate = ((x) => x.StartDate); break;
                case "2":
                    orderByPredicate = ((x) => x.EndDate); break;
                default:
                    orderByPredicate = ((x) => x.StartDate); break;
            }

            var datatableSpecificationData = new ConvocationCalendarDatatableSpecification(sentParameters, orderByPredicate, convocationId);
            var datatableSpecificationCount = new ConvocationCalendarFilterSpecification(convocationId);

            var data = await _convocationCalendarRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _convocationCalendarRepository.CountAsync(datatableSpecificationCount);
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
