namespace TechECommerceServer.Domain.DTOs.AppUser
{
    public class LogInAppUserRequestDto
    {
        public string UserNameOrEmail { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
