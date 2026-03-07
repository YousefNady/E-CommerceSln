using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services.Specifications;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.ProductDTOs;

namespace E_Commerce.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<BrandDTO>>(brands);
        }

        public async Task<PaginatedResult<ProductDTO>> GetAllProductsAsync(ProductQueryParams queryParams)
        {
            var Repo = _unitOfWork.GetRepository<Product, int>();
            // Add Filter with BrandId or TypeId If Needed
            var Spec = new ProductWithTypeAndBrandSpecification(queryParams);
            var products = await Repo.GetAllAsync(Spec);
            var DataToReturn = _mapper.Map<IEnumerable<ProductDTO>>(products);
            var CountOfReturnedData = DataToReturn.Count();
            var CountSpec = new ProductCountSpecifications(queryParams);
            var CountOfAllProducts = await Repo.CountAsync(CountSpec);
            return new PaginatedResult<ProductDTO>(queryParams.PageIndex, CountOfReturnedData, CountOfAllProducts, DataToReturn);
        }

        public async Task<IEnumerable<TypeDTO>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeDTO>>(Types);
        }

        public async Task<Result<ProductDTO>> GetProductByIdAsync(int id)
        {
            var Spec = new ProductWithTypeAndBrandSpecification(id);
            var Product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec);
            if (Product is null)
                return Error.NotFound("Product.NotFound", $"Product With Id {id} Is Not Found");
            return _mapper.Map<ProductDTO>(Product);

            // implicitly casting the ProductDTO to Result<ProductDTO> using the implicit operator defined in the Result class
            // Result<ProductDTO>.Fail -  Result<ProductDTO>.Ok
        }
    }
}
