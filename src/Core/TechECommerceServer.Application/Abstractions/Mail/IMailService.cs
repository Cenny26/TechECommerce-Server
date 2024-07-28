namespace TechECommerceServer.Application.Abstractions.Mail
{
    public interface IMailService
    {
        Task SendMailAsync(string to, string subject, string body, bool isBodyHtml = true);
        Task SendMailAsync(string[] tos, string subject, string body, bool isBodyHtml = true);
        Task SendPasswordResetDemandMailAsync(string to, string userId, string resetToken);
    }
}
