using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence
{
    internal static class SpecificationEvaluator
    {
        // The Objective : Create Query Based On Specification - Bulid Query from Specification
        // _context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand);

        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> EntryPoint, 
            ISpecification<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = EntryPoint; //  _context.(AnyEntity)Products
            if (specifications is not null)
            {
                if (specifications.IncludeExpression is not null && specifications.IncludeExpression.Any())
                {
                    if (specifications.Criteria is not null)
                    {
                        Query = Query.Where(specifications.Criteria);
                    }

                    if (specifications.IncludeExpression is not null && specifications.IncludeExpression.Any())
                    {
                        Query = specifications.IncludeExpression.Aggregate(Query,  // Instead of using foreach we can use Aggregate Linq Method
                                (CurrentQuery, IncludeQuery) => CurrentQuery.Include(IncludeQuery));
                        //_context.Products.Include(p => p.ProductType).Include(p => p.ProductBrand);
                    }

                    if (specifications.OrderBy is not null)
                    {
                        Query = Query.OrderBy(specifications.OrderBy);
                    }

                    if (specifications.OrderByDescending is not null)
                    {
                        Query = Query.OrderByDescending(specifications.OrderByDescending);
                    }

                    if (specifications.IsPaginated)
                    {
                        Query = Query.Skip(specifications.Skip).Take(specifications.Take);
                    }

                }
            }

            return Query;
        }
    }
}
