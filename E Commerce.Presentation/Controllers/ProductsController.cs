using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.DTOs.ProductDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        // get : baseUrl/api/products / 20
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProdcuts()
        {
            var products = await productService.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        // get : baseUrl/api/products
        public async Task<ActionResult<ProductDTO>> GetProduct(int id)
        {
            var Product = await productService.GetProductByIdAsync(id);
            return Ok(Product);
        }

        [HttpGet("types")]
        // get : baseUrl/api/Products/types
        public async Task<ActionResult<IEnumerable<TypeDTO>>> GetAllTypes()
        {
            var Types = await productService.GetAllTypesAsync();
            return Ok(Types);
        }

        [HttpGet("brands")]
        // get : baseUrl/api/Products/brands
        public async Task<ActionResult<IEnumerable<BrandDTO>>> GetAllBrands()
        {
            var Brands = await productService.GetAllBrandsAsync();
            return Ok(Brands);
        }


    }
}
