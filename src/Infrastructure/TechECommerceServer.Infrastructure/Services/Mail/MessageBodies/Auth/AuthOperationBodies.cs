using System.Text;

namespace TechECommerceServer.Infrastructure.Services.Mail.MessageBodies.Auth
{
    public static class AuthOperationBodies
    {
        public static string GetPasswordResetMailBody(string angularClientUrl, string userId, string resetToken)
        {
            StringBuilder mail = new StringBuilder();
            mail.Append("Hello!" +
                "<br>" +
                "If you have requested a new password, you can update your password from the link below." +
                "<br><strong><a target=\"_blank\" href=\"");
            mail.Append(angularClientUrl);
            mail.Append("/update-password/");
            mail.Append(userId);
            mail.Append("/");
            mail.Append(resetToken);
            mail.Append("\">Click here to request a new password!</a></strong>" +
                "<br>" +
                "<span>NOTE: If this request is not fulfilled by you, please do not take this message seriously.</span>" +
                "<br>" +
                "Sincerely...");

            return mail.ToString();
        }
    }
}
