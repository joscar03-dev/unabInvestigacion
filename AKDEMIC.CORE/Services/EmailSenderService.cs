using AKDEMIC.CORE.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AKDEMIC.CORE.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        public Task SendEmailAsync(string email, string subject , string message, string projectName)
        {
            var senderEmail = InstitutionConstants.SupportEmail[GeneralConstants.Institution.VALUE];
            var senderEmailPassword = InstitutionConstants.SupportEmailPassword[GeneralConstants.Institution.VALUE];

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, projectName)
            };

            mailMessage.To.Add(new MailAddress(email));
            mailMessage.Subject = subject;
            mailMessage.Body = message;
            mailMessage.IsBodyHtml = true;

            using (var client = new SmtpClient("smtp.gmail.com", 587))
            {
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(mailMessage.From.Address, senderEmailPassword);
                client.EnableSsl = true;
                client.Send(mailMessage);
            }

            return Task.CompletedTask;
        }

        public async Task SendEmailPasswordRecoveryAsync(string projectName, string email, string url, string callbackUrl)
        {
            var emailhelper = new AKDEMIC.CORE.Helpers.EmailHelper();
            await SendEmailAsync(email, $"Recuperar contraseña en {projectName}", emailhelper.ForgotPassword(projectName, url, callbackUrl), projectName);
        }

        public async Task SendEmailProjectReport(string projectName, string email, string url, string callbackUrl)
        {
            var emailhelper = new AKDEMIC.CORE.Helpers.EmailHelper();
            await SendEmailAsync(email, $"Subir entregable pronto a vencer {projectName}", emailhelper.SendEmailReport(projectName, url, callbackUrl), projectName);
        }


    }
}
