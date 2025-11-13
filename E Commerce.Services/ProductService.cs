using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public ProductService(IUnitOfWork unitOfWork , IMapper mapper )
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return mapper.Map<IEnumerable<BrandDTO>>(brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = unitOfWork.GetRepository<Product, int>();
            // Add Filter with BrandId or TypeId If Needed
            var Spec = new ProductWithTypeAndBrandSpecification(queryParams);
            var products = await Repo.GetAllAsync(Spec);
            var DataToReturn = mapper.Map<IEnumerable<ProductDTO>>(products);
            var CountOfReturnedData = DataToReturn.Count();
            var CountSpec = new ProductCountSpecifications(queryParams);
            var CountOfAllProducts = await Repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOfReturnedData, CountOfAllProducts, DataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<ProductDTO> GetProductByIdAsync(int id) 
        {
            var Spec = new ProductWithTypeAndBrandSpecification(id);
            var Product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);
            return mapper.Map<ProductDTO>(Product);
        }
    }
}
