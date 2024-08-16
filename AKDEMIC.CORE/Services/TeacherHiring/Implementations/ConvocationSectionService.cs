using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationSectionSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationSectionService : IConvocationSectionService
    {
        private readonly IAsyncRepository<ConvocationSection> _convocationSectionRepository;

        public ConvocationSectionService(
            IAsyncRepository<ConvocationSection> convocationSectionRepository
            )
        {
            _convocationSectionRepository = convocationSectionRepository;
        }

        public async Task<bool> AnyByTitle(string title,Guid convocationId, Guid? ignoredId = null)
        {
            var sectionSpecification = new ConvocationSectionFilterSpecification(title, convocationId, ignoredId);
            var section = await _convocationSectionRepository.FirstOrDefaultAsync(sectionSpecification);

            return section != null;
        }

        public async Task<IReadOnlyList<ConvocationSection>> GetAllByConvocationId(Guid convocationId)
        {
            var sectionSpecification = new ConvocationSectionFilterSpecification(convocationId);
            var data = await _convocationSectionRepository.ListAsync(sectionSpecification);
            return data;
        }
    }
}
