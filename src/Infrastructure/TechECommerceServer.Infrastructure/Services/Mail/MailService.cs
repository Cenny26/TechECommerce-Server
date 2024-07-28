using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using TechECommerceServer.Application.Abstractions.Mail;
using TechECommerceServer.Infrastructure.Services.Mail.MessageBodies.Auth;

namespace TechECommerceServer.Infrastructure.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendMailAsync(new string[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mailMessage = new MailMessage();

            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = isBodyHtml;

            foreach (string to in tos)
                mailMessage.To.Add(to);

            mailMessage.From = new MailAddress(_configuration["Mail:UserName"]!, _configuration["Mail:DisplayName"], System.Text.Encoding.UTF8);

            SmtpClient smtpClient = new SmtpClient(_configuration["Mail:Host"], int.Parse(_configuration["Mail:Port"]!));
            smtpClient.Credentials = new NetworkCredential(_configuration["Mail:UserName"], _configuration["Mail:Password"]);
            smtpClient.EnableSsl = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendPasswordResetDemandMailAsync(string to, string userId, string resetToken)
        {
            string angularClientUrl = _configuration["BaseUrls:ClientUrls:AngularClientUrl"]!;
            string mailBody = AuthOperationBodies.GetPasswordResetMailBody(angularClientUrl, userId, resetToken);

            await SendMailAsync(to, "Password Renewal Request!", mailBody);
        }
    }
}
