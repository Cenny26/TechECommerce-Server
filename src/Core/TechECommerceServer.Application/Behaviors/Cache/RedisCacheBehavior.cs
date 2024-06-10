using MediatR;
using TechECommerceServer.Application.Abstractions.Cache;

namespace TechECommerceServer.Application.Behaviors.Cache
{
    public class RedisCacheBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRedisCacheService _redisCacheService;
        public RedisCacheBehavior(IRedisCacheService redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (request is ICacheableQuery query)
            {
                string cacheKey = query.CacheKey;
                double cacheTime = query.CacheTime;

                TResponse cacheData = await _redisCacheService.GetAsync<TResponse>(cacheKey);
                if (cacheData is not null)
                    return cacheData;

                TResponse response = await next();
                if (response is not null)
                    await _redisCacheService.SetAsync(cacheKey, response, DateTime.Now.AddMinutes(cacheTime));

                return response;
            }

            return await next();
        }
    }
}
