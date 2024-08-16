using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string email, string subject, string message, string projectName);
        Task SendEmailProjectReport(string projectName, string email, string url, string callbackUrl);
        Task SendEmailPasswordRecoveryAsync(string projectName, string email, string url, string callbackUrl);
    }
}
