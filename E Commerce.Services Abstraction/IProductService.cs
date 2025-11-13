using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services_Abstraction
{
    public  interface IProductService
    {
        // Get All Products Return IEnumerable Of Products Data Which Will be 
        // Name, Description , PictureUrl , Price , ProductBrand, ProductType
        Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams);

        // Get Product By Id Return Product Data Which Will be 
        // Name, Description , PictureUrl , Price , ProductBrand, ProductType
        Task<ProductDTO> GetProductByIdAsync(int id);

        // Get All Brands Return IEnumerable Of Brands Data Which Will be 
        // Name
        Task<IEnumerable<BrandDTO>> GetAllBrandsAsync();

        // Get All Types Return IEnumerable Of Types Data Which Will be 
        // Name
        Task<IEnumerable<TypeDTO>> GetAllTypesAsync();
    }
}
