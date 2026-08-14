using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.BasketModule;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Persistence.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase _database;

        public BasketRepository(IConnectionMultiplexer connection) // asking clr to inject the Cashing Services (To Deal With RAM)
        {
            _database = connection.GetDatabase();
        }

        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan TimeToLive = default)
        {
            var JsonBasket = JsonSerializer.Serialize(basket);
            var IsCreatedOrUpdated = await _database.StringSetAsync(basket.Id, JsonBasket,
                (TimeToLive == default) ? TimeSpan.FromDays(7) : TimeToLive);
            if (IsCreatedOrUpdated)
            {
                  return await GetBasketAsync(basket.Id);
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> DeleteBasketAsync(string BasketId) => await _database.KeyDeleteAsync(BasketId);

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket = await _database.StringGetAsync(BasketId);
            if (Basket.IsNullOrEmpty) return null;
            else
            {
                return JsonSerializer.Deserialize<CustomerBasket>(Basket!);
            }
        }
    }
}
