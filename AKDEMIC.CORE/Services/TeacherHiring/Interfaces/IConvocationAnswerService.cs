using AKDEMIC.DOMAIN.Entities.TeacherHiring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationAnswerService
    {
        Task<IReadOnlyList<ConvocationAnswer>> GetAllByQuestionId(Guid questionId);
        Task<IReadOnlyList<ConvocationAnswer>> GetAllByQuestionsId(List<Guid> questionsId);
    }
}
