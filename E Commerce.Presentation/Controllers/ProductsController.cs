using E_Commerce.Presentation.Attributes;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Presentation.Controllers
{
    public class ProductsController : ApiBaseController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [RedisCache]
        // get : baseUrl/api/products / 20
        public async Task<ActionResult<PaginatedResult<ProductDTO>>> GetAllProducts([FromQuery] ProductQueryParams queryParams)
        {
            var products = await _productService.GetAllProductsAsync(queryParams);
            return Ok(products);
        }

        // get product by id
        [HttpGet("{id}")]
        // get : baseUrl/api/products/1
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var Product = await _productService.GetProductByIdAsync(id);
            return HandleResult<ProductDTO>(Product);
        }

        [HttpGet("types")]
        // get : baseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await _productService.GetAllTypesAsync();
            return Ok(Types);
        }

        [HttpGet("brands")]
        // get : baseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await _productService.GetAllBrandsAsync();
            return Ok(Brands);
        }


    }
}
