using System.Text.Json.Serialization;

namespace TechECommerceServer.Domain.DTOs.Auth.Facebook
{
    public class FacebookUserAccessTokenValidationResponseDto
    {
        [JsonPropertyName("data")]
        public FacebookUserAccessTokenValidationDataResponseDto Data { get; set; }
    }
}
