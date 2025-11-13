using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Contracts
{
    public interface ISpecification <TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity,object>>> IncludeExpression { get;} // ana m4 3wzo y8er fe al object el ht3mlo Create
                                                                                       // ana h3mlo Method 34an y7ot gwha ay 7ga 

        public Expression<Func<TEntity,bool>> Criteria { get; }
        public Expression<Func<TEntity, object>> OrderBy { get; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
        public int Take { get;}
        public int Skip { get; }
        public bool IsPaginated { get; }


    }
}
