using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationRubricItemSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationRubricItemService : IConvocationRubricItemService
    {
        private readonly IAsyncRepository<ConvocationRubricItem> _convocationRubricItemRepository;

        public ConvocationRubricItemService(
            IAsyncRepository<ConvocationRubricItem> convocationRubricItemRepository
            )
        {
            _convocationRubricItemRepository = convocationRubricItemRepository;
        }

        public async Task<IReadOnlyList<ConvocationRubricItem>> GetAllBySectionId(Guid convocationRubricSectionId)
        {
            var questionSpecification = new ConvocationRubricItemFilterSpecification(convocationRubricSectionId);
            var items = await _convocationRubricItemRepository.ListAsync(questionSpecification);
            return items;
        }

        public async Task<IReadOnlyList<ConvocationRubricItem>> GetAllBySectionsId(List<Guid> convocationRubricSectionsId)
        {
            var questionSpecification = new ConvocationRubricItemFilterSpecification(convocationRubricSectionsId);
            var items = await _convocationRubricItemRepository.ListAsync(questionSpecification);
            return items;
        }

    }
}
