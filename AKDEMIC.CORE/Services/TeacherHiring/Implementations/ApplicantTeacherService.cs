using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ApplicantTeacherSpecifications;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ApplicantTeacherService : IApplicantTeacherService
    {
        private readonly IAsyncRepository<ApplicantTeacher> _applicantTeacherRepository;

        public ApplicantTeacherService(IAsyncRepository<ApplicantTeacher> applicantTeacherRepository)
        {
            _applicantTeacherRepository = applicantTeacherRepository;
        }

        public async Task<DataTablesStructs.ReturnedData<object>> GetApplicantTeachersDatatable(DataTablesStructs.SentParameters parameters, Guid? convocationId, string search, ClaimsPrincipal user = null)
        {
            Expression<Func<ApplicantTeacher, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.User.FullName); break;
                case "1":
                    orderByPredicate = ((x) => x.User.Dni); break;
                case "2":
                    orderByPredicate = ((x) => x.ConvocationVacancy.AcademicDepartmentText); break;
                case "3":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "4":
                    orderByPredicate = ((x) => x.Convocation.Name); break;
                case "5":
                    orderByPredicate = ((x) => x.Status); break;
                case "6":
                    orderByPredicate = ((x) => x.Valid); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var datatableSpecificationData = new ApplicantTeacherDatatableSpecification(parameters, orderByPredicate, convocationId, search, user);
            var datatableSpecificationCount = new ApplicantTeacherFilterSpecification(convocationId, search, user, null);

            var data = await _applicantTeacherRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _applicantTeacherRepository.CountAsync(datatableSpecificationCount);
            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }

        public async Task<bool> AnyByConvocation(Guid convocationId)
        {
            var filterSpecfication = new ApplicantTeacherFilterSpecification(convocationId, null, null, null);
            var data = await _applicantTeacherRepository.CountAsync(filterSpecfication);
            return data > 0;
        }

        public async Task<int> GetPostulantsCountByConvocationVacancy(Guid convocationVacancyId)
        {
            var filterSpecfication = new ApplicantTeacherFilterSpecification(null, null, null, convocationVacancyId);
            var data = await _applicantTeacherRepository.CountAsync(filterSpecfication);
            return data;
        }
    }
}
