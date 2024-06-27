using System.Text.Json.Serialization;

namespace TechECommerceServer.Domain.DTOs.Auth.Facebook
{
    public class FacebookUserAccessTokenValidationDataResponseDto
    {
        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }
    }
}
