using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    internal class ProductWithTypeAndBrandSpecification : BaseSpecifications<Product, int>
    {
        // get product By id

        public ProductWithTypeAndBrandSpecification(int id) : base(p => p.Id == id)
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
        }
        // get all products with their types and brands
        public ProductWithTypeAndBrandSpecification(ProductQueryParams queryParams) : base (ProductSpecificationsHelper.GetProductCriteria(queryParams))
            

         //True && True
         //True && True
         //p.BrandId == BrandId.Value  && p.TypeId == TypeId.Value
         //p => p.BrandId == BrandId -> BrandId Is Not Null
         //p => p.TypeId == TypeId -> TypeId Is Not Null
         //p => p.BrandId == BrandId && p.TypeId == TypeId  -> BrandId and TypeId Is Not Null
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);

            switch(queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;

            }

            ApplyPagination(queryParams.PageSize, queryParams.PageIndex);
        }
    }
}
