using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationRubricSectionSpecification;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationRubricSectionService : IConvocationRubricSectionService
    {
        private readonly IAsyncRepository<ConvocationRubricSection> _convocationRubricSection;

        public ConvocationRubricSectionService(
            IAsyncRepository<ConvocationRubricSection> convocationRubricSection
            )
        {
            _convocationRubricSection = convocationRubricSection;
        }

        public async Task<IReadOnlyList<ConvocationRubricSection>> GetAllByConvocationId(Guid convocationId, byte type)
        {
            var sectionSpecification = new ConvocationRubricSectionFilterSpecification(convocationId, type);
            var data = await _convocationRubricSection.ListAsync(sectionSpecification);
            return data;
        }
    }
}
