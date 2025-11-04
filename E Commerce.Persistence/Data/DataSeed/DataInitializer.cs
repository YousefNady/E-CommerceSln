using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Data.DataSeed
{
    public class DataInitializer : IDataInitializer
    {
        private readonly StoreDbContext dbContext;

        public DataInitializer(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                var HasProducts = await dbContext.Products.AnyAsync();
                var HasProductBrands = await dbContext.ProductBrands.AnyAsync();
                var HasProductTypes = await dbContext.ProductTypes.AnyAsync();
                if (HasProducts && HasProductBrands && HasProductTypes) return; // exit the function [Don't continue]

                if (!HasProductBrands)
                {
                   await seedDataFromJsonAsync<ProductBrand, int>("brands.json", dbContext.ProductBrands); // Add Locally
                }
                if (!HasProductTypes)
                {
                   await seedDataFromJsonAsync<ProductType, int>("types.json", dbContext.ProductTypes); 
                }
                await dbContext.SaveChangesAsync(); // commit to Database

                if (!HasProducts)
                {
                   await seedDataFromJsonAsync<Product, int>("products.json", dbContext.Products);
                }
                await dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Data Seeding Failed : {ex}");
            }
        }

        private async Task seedDataFromJsonAsync<T, TKey>(string fileName, DbSet<T> dbset) where T : BaseEntity<TKey>
        {
            // E Commerce.Persistence\Data\DataSeed\JSONFiles\brands.json
            var filePath = @"E Commerce.Persistence\Data\DataSeed\JSONFiles\" + fileName;
            if (!File.Exists(filePath)) throw new FileNotFoundException($"File {fileName} Is Not Exists");
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
            catch(Exception ex)
            {

                Console.WriteLine($"Error While Reading Json File : {ex} ");
            }


        }
    }
}
