using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    // ana 3mltha 34an tb2a Common lw 7bt t3ml ay Specification tany f ay Service tany
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecifications(Expression<Func<TEntity, bool>> CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }
        public Expression<Func<TEntity, bool>> Criteria  { get; }


        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];
        // method 34an y3ml add le include expressions mn 5lalha (With My Way)
        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExp)
        {
            IncludeExpression.Add(IncludeExp); // hena b3ml add le kol expression b3to 34an y7otohom fe al collection (First Method)
        }
        #endregion

        #region Sorting
        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }


        protected void AddOrderBy(Expression<Func<TEntity, object>> OrderByExpression)
        {
            OrderBy = OrderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> OrderByDescendingExpression)
        {
            OrderBy = OrderByDescendingExpression;
        }

        #endregion

        #region Pagination
        public int Take  { get; private set; }

        public int Skip { get; private set; }

        public bool IsPaginated { get; private set; }

        // Total Count = 40
        // pageSize = 10
        // 10, 10, 10, 10
        // PageIndex => 3
        protected void ApplyPagination(int pageSize,int pageIndex)
        {
            IsPaginated = true;
            Take = pageSize;
            Skip = (pageIndex - 1) * pageSize; //(ask for 3) =>  3-1 * 10 => skip the first 20 Product

        }
        #endregion
    }
}
