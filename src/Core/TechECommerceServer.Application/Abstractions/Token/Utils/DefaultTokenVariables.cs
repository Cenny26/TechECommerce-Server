namespace TechECommerceServer.Application.Abstractions.Token.Utils
{
    public static class DefaultTokenVariables
    {
        // Standard access token duration in minutes (e.g., 60 minutes).
        public static int StandardAccessTokenValueByMinutes = 60;

        // Standard access token duration in seconds, calculated from minutes.
        public static int StandardAccessTokenValueBySeconds = StandardAccessTokenValueByMinutes * 60;

        // Standard refresh token duration in minutes (e.g., 75 minutes).
        public static int StandardRefreshTokenValueByMinutes = 75;

        // Standard refresh token duration in seconds, calculated from minutes.
        public static int StandardRefreshTokenValueBySeconds = StandardRefreshTokenValueByMinutes * 60;
    }
}
