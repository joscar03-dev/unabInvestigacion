using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services.General.Interfaces
{
    public interface IConfigurationService
    {
        Task<string> GetValueByKey(string key);
    }
}
