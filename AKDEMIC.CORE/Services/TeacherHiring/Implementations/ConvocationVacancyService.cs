using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationVacancySpecification;
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
    public class ConvocationVacancyService : IConvocationVacancyService
    {
        private readonly IAsyncRepository<ConvocationVacancy> _convocationVacannyRepository;

        public ConvocationVacancyService(
            IAsyncRepository<ConvocationVacancy> convocationVacannyRepository
            )
        {
            _convocationVacannyRepository = convocationVacannyRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetConvocationVacanciesDatatable(DataTablesStructs.SentParameters sentParameters, Guid convocationId, Guid? academicDepartmentId)
        {
            Expression<Func<ConvocationVacancy, dynamic>> orderByPredicate = null;
            switch (sentParameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "1":
                    orderByPredicate = ((x) => x.AcademicDepartmentText); break;
                case "2":
                    orderByPredicate = ((x) => x.Vacancies); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var datatableSpecificationData = new ConvocationVacancyDatatableSpecification(sentParameters, orderByPredicate, convocationId, academicDepartmentId);
            var datatableSpecificationCount = new ConvocationVacancyFilterSpecification(convocationId, academicDepartmentId);

            var data = await _convocationVacannyRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _convocationVacannyRepository.CountAsync(datatableSpecificationCount);
            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = sentParameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };
        }

        public async Task<IReadOnlyList<ConvocationVacancy>> GetAllByConvocationId(Guid convocationId)
        {
            var filterSpecification = new ConvocationVacancyFilterSpecification(convocationId, null);
            var result = await _convocationVacannyRepository.ListAsync(filterSpecification);

            return result;
        }
    }
}
