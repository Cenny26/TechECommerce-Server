namespace TechECommerceServer.Domain.DTOs.Auth
{
    public sealed class Token
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
