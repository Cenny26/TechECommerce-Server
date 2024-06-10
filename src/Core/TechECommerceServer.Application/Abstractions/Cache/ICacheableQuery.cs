namespace TechECommerceServer.Application.Abstractions.Cache
{
    public interface ICacheableQuery
    {
        public string CacheKey { get; }
        public double CacheTime { get; }
    }
}
