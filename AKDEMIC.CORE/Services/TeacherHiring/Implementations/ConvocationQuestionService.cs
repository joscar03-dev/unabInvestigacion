using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationQuestionSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationQuestionService : IConvocationQuestionService
    {
        private readonly IAsyncRepository<ConvocationQuestion> _convocationQuestionRepository;

        public ConvocationQuestionService(
            IAsyncRepository<ConvocationQuestion> convocationQuestionRepository
            )
        {
            _convocationQuestionRepository = convocationQuestionRepository;
        }

        public async Task<IReadOnlyList<ConvocationQuestion>> GetAllBySectionId(Guid convocationSectionId)
        {
            var questionSpecification = new ConvocationQuestionFilterSpecification(convocationSectionId);
            var questions = await _convocationQuestionRepository.ListAsync(questionSpecification);
            return questions;
        }

        public async Task<bool> AnyByDescription(Guid convocationSectionId, string description, Guid? ignoredId = null)
        {
            var questionSpecification = new ConvocationQuestionFilterSpecification(convocationSectionId, description, ignoredId);
            var question = await _convocationQuestionRepository.FirstOrDefaultAsync(questionSpecification);
            return question != null;
        }

        public async Task<IReadOnlyList<ConvocationQuestion>> GetAllBySectionsId(List<Guid> convocationSectionId)
        {
            var questionSpecification = new ConvocationQuestionFilterSpecification(convocationSectionId);
            var questions = await _convocationQuestionRepository.ListAsync(questionSpecification);
            return questions;
        }
    }
}
