using E_Commerce.Domain.Contracts;
using E_Commerce.Services_Abstraction;
using System.Text.Json;

namespace E_Commerce.Services
{
    public class CacheService : ICacheService
    {
        private readonly ICacheRepository _cacheRepository;

        public CacheService(ICacheRepository cacheRepository)
        {
            _cacheRepository = cacheRepository;
        }
        public async Task<string?> GetAsync(string CacheKey)
        {
            return await _cacheRepository.GetAsync(CacheKey);
        }

        public async Task SetAsync(string CacheKey, object CacheValue, TimeSpan TimeToLive)
        {
            var value = JsonSerializer.Serialize(CacheValue, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase 
            }); // Serialize Object to String
            await _cacheRepository.SetAsync(CacheKey, value, TimeToLive);
        }
    }
}
