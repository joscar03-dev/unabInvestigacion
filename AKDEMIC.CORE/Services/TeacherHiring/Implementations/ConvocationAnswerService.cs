using AKDEMIC.CORE.Interfaces;
using AKDEMIC.CORE.Services.TeacherHiring.Interfaces;
using AKDEMIC.CORE.Specifications.TeacherHiring.ConvocationAnswerSpecifications;
using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Implementations
{
    public class ConvocationAnswerService : IConvocationAnswerService
    {
        private readonly IAsyncRepository<ConvocationAnswer> _convocationAnswerRepository;

        public ConvocationAnswerService(
            IAsyncRepository<ConvocationAnswer> convocationAnswerRepository
            )
        {
            _convocationAnswerRepository = convocationAnswerRepository;
        }

        public async Task<IReadOnlyList<ConvocationAnswer>> GetAllByQuestionId(Guid questionId)
        {
            var answerSpecification = new ConvocationAnswerFilterSpecification(questionId);
            var data = await _convocationAnswerRepository.ListAsync(answerSpecification);
            return data;
        }

        public async Task<IReadOnlyList<ConvocationAnswer>> GetAllByQuestionsId(List<Guid> questionsId)
        {
            var answerSpecification = new ConvocationAnswerFilterSpecification(questionsId);
            var data = await _convocationAnswerRepository.ListAsync(answerSpecification);
            return data;
        }

    }
}
