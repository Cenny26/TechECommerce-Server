namespace TechECommerceServer.Domain.DTOs.AppUser.UpdatePassword
{
    public class UpdatePasswordRequestDto
    {
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
