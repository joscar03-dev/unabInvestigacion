using AKDEMIC.CORE.DTOs.TeacherHiring.ConvocationComitee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.TeacherHiring.Interfaces
{
    public interface IConvocationComiteeService
    {
        Task<List<ComiteeDto>> GetComitee(Guid convocationId);
    }
}
