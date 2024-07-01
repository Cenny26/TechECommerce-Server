namespace TechECommerceServer.Application.Abstractions.Token.Utils
{
    public static class DefaultTokenVariables
    {
        public static int StandardAccessTokenValueByMinutes = 60;
        public static int StandardAccessTokenValueBySeconds = 60 * 60;
        public static int StandardRefreshTokenValueByMinutes = 15;
        public static int StandardRefreshTokenValueBySeconds = 15 * 60;
    }
}
