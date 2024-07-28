namespace TechECommerceServer.Domain.DTOs.Auth.VerifyResetToken
{
    public class VerifyResetTokenRequestDto
    {
        public string ResetToken { get; set; }
        public string UserId { get; set; }
    }
}
