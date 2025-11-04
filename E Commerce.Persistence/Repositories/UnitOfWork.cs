using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Data.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext dbContext;
        private readonly Dictionary<Type, object> repositories = [];

        public UnitOfWork(StoreDbContext DbContext)
        {
            dbContext = DbContext;
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var EntityType = typeof(TEntity);
            if (repositories.TryGetValue(EntityType,out var repository))
               return (IGenericRepository<TEntity, TKey>) repository;

            var newRepo = new GenericRepository<TEntity, TKey>(dbContext);
            repositories[EntityType] = newRepo;
            return newRepo;
        }

        public async Task<int> SaveChangesAsync() => await dbContext.SaveChangesAsync();
    }
}
