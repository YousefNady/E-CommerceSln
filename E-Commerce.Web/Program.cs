
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Persistence.Data.DataSeed;
using E_Commerce.Persistence.Data.DbContexts;
using E_Commerce.Persistence.Repositories;
using E_Commerce.Services;
using E_Commerce.Services.MappingProfiles;
using E_Commerce.Services_Abstraction;
using E_Commerce.Web.Extensions;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace E_Commerce.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            #region Add services to the container

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddScoped<IDataInitializer, DataInitializer>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            //builder.Services.AddAutoMapper(x => x.LicenseKey = "",typeof(ProductProfile).Assembly);
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddAutoMapper(typeof(ProductProfile).Assembly);
            builder.Services.AddSingleton<IConnectionMultiplexer>(SP =>
            {
                return ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString(" Connection")!);
            });
            builder.Services.AddScoped<IBasketRepository, BasketRepository>();
            builder.Services.AddScoped<IBasketService, BasketService>();
            builder.Services.AddScoped<ICacheRepository, CacheRepository>();
            builder.Services.AddScoped<ICacheService, CacheService>();
            #endregion

            var app = builder.Build();

            #region DataSeeding
            await app.MigrateDatabase();
            await app.SeedDatabase();
            #endregion


            #region Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseStaticFiles();
            app.MapControllers(); 

            #endregion

            await app.RunAsync();
        }
    }
}
