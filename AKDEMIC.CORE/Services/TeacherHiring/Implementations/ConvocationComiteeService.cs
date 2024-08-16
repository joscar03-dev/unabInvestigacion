using AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationComitee;
using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationComiteeSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationComiteeService : IConvocationComiteeService
    {
        private readonly IAsyncRepository<ConvocationComitee> _convocationComiteeRepository;

        public ConvocationComiteeService(
            IAsyncRepository<ConvocationComitee> convocationComiteeRepository
            )
        {
            _convocationComiteeRepository = convocationComiteeRepository;
        }

        public async Task<List<ComiteeDto>> GetComitee(Guid convocationId)
        {
            var comiteeSpecification = new ConvocationComiteeDataSpecification(convocationId);
            var data = await _convocationComiteeRepository.ListAsync(comiteeSpecification);
            return data;
        }
    }
}
