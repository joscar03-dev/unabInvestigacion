using AKDEMIC.CORE.Constants;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.General.Interfaces;
using AKDEMIC.CORE.Specifications.General.ApplicationUserSpecifications;
using AKDEMIC.CORE.Structs;
using AKDEMIC.DOMAIN.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.General.Implementations
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IAsyncRepository<ApplicationUser> _applicationUserRepository;

        public ApplicationUserService(IAsyncRepository<ApplicationUser> applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }

        public async Task<bool> UserExistsByUserName(string userName = null)
        {
            var userFilterSpecification = new ApplicationUserFilterSpecification(userName);

            var applicationUser = await _applicationUserRepository.FirstOrDefaultAsync(userFilterSpecification, ignoreQueryFilter: true);

            if (applicationUser == null)
                return false;
            else
                return true;
        }

        public async Task<Select2Structs.ResponseParameters> GetSelect2(Select2Structs.RequestParameters requestParameters, List<string> ignoredRoles = null, List<string> selectedRoles = null)
        {
            var selectSpecification = new ApplicationUserSelectSpecification(requestParameters, ignoredRoles, selectedRoles);
            var data = await _applicationUserRepository.ListAsync(selectSpecification);

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

        public async Task<DataTablesStructs.ReturnedData<object>> GetUsersDatatable(DataTablesStructs.SentParameters parameters, List<string> roles, string search)
        {
            Expression<Func<ApplicationUser, dynamic>> orderByPredicate = null;
            switch (parameters.OrderColumn)
            {
                case "0":
                    orderByPredicate = ((x) => x.CreatedAt); break;
                case "1":
                    orderByPredicate = ((x) => x.FullName); break;
                case "2":
                    orderByPredicate = ((x) => x.UserName); break;
                case "3":
                    orderByPredicate = ((x) => x.Email); break;
                default:
                    orderByPredicate = ((x) => x.CreatedAt); break;
            }

            var datatableSpecificationData = new ApplicationUserDatatableSpecification(parameters, orderByPredicate, roles, search);
            var datatableSpecificationCount = new ApplicationUserFilterSpecification(roles, search);

            var data = await _applicationUserRepository.ListAsync(datatableSpecificationData);
            var recordsFiltered = await _applicationUserRepository.CountAsync(datatableSpecificationCount);
            int recordsTotal = data.Count;

            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters.DrawCounter,
                RecordsFiltered = recordsFiltered,
                RecordsTotal = recordsTotal
            };

        }
    }
}
