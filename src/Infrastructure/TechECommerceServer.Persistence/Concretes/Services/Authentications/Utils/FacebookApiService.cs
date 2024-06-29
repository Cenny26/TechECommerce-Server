namespace TechECommerceServer.Persistence.Concretes.Services.Authentications.Utils
{
    public static class FacebookApiService
    {
        public static string BuildAccessTokenUrl(string clientId, string clientSecret, string grantType)
        {
            UriBuilder uriBuilder = new UriBuilder("https://graph.facebook.com/oauth/access_token")
            {
                Query = $"client_id={clientId}&client_secret={clientSecret}&grant_type={grantType}"
            };
            return uriBuilder.ToString();
        }

        public static string BuildUserAccessTokenValidationUrl(string authToken, string accessToken)
        {
            UriBuilder uriBuilder = new UriBuilder("https://graph.facebook.com/debug_token")
            {
                Query = $"input_token={authToken}&access_token={accessToken}"
            };
            return uriBuilder.ToString();
        }

        public static string BuildUserInfoUrl(string authToken)
        {
            UriBuilder uriBuilder = new UriBuilder("https://graph.facebook.com/me")
            {
                Query = $"fields=email,name&access_token={authToken}"
            };
            return uriBuilder.ToString();
        }
    }
}
