using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await _dbContext.Products.AnyAsync();
                var HasProductBrands = await _dbContext.ProductBrands.AnyAsync();
                var HasProductTypes = await _dbContext.ProductTypes.AnyAsync();
                var HasDeliveryMethods = await _dbContext.Set<DeliveryMethod>().AnyAsync();
                if (HasProducts && HasProductBrands && HasProductTypes && HasDeliveryMethods) return; // exit the function [Don't continue]

                if (!HasProductBrands)
                {
                    await seedDataFromJsonAsync<ProductBrand, int>("brands.json", _dbContext.ProductBrands); // Add Locally
                }
                if (!HasProductTypes)
                {
                    await seedDataFromJsonAsync<ProductType, int>("types.json", _dbContext.ProductTypes);
                }
                await _dbContext.SaveChangesAsync(); // commit to Database

                if (!HasProducts)
                {
                    await seedDataFromJsonAsync<Product, int>("products.json", _dbContext.Products);
                }
                if (!HasDeliveryMethods)
                {
                    await seedDataFromJsonAsync<DeliveryMethod, int>("delivery.json", _dbContext.Set<DeliveryMethod>());
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding Failed : {ex}");
            }
        }

        private async Task seedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            // E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json
            var filePath = @"..\E Commerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"[Error] File not found at: {Path.GetFullPath(filePath)}");
                return;
            }
            try
            {
                using var dataStream = File.OpenRead(filePath);
                var data = await JsonSerializer.DeserializeAsync<List<T>>(dataStream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (data is not null)
                {
                    await dbset.AddRangeAsync(data);
                }


            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error While Reading Json File : {ex} ");
            }


        }
    }
}
