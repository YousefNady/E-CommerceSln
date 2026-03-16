using AutoMapper;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.OrderModule;
using E_Commerce.Domain.Entities.ProductModule;
using E_Commerce.Services_Abstraction;
using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Services
{
    public class OrderService : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IMapper mapper, IBasketRepository basketRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email)
        {
            var OrderAddress = _mapper.Map<OrderAddress>(orderDTO.Address);

            var Basket = await _basketRepository.GetBasketAsync(orderDTO.BasketId);
            if (Basket is null) return Error.NotFound("Basket.NotFound", $"The Basket With Id {orderDTO.BasketId} Is Not Found");

            List<OrderItem> OrderItems = new List<OrderItem>();
            foreach (var item in Basket.Items)
            {
                var product = await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(item.Id);
                if (product is null) return Error.NotFound("Product.NotFound", $"The Product With Id {item.Id} Is Not Found");
                OrderItems.Add(CreateOrderItem(item, product));
            }

            var DeliveryMethod = await _unitOfWork.GetRepository<DeliveryMethod, int>().GetByIdAsync(orderDTO.DeliveryMethodId);
            if (DeliveryMethod is null) return Error.NotFound("DeliveryMethod.NotFound",
                $"The Delivery Method With Id {orderDTO.DeliveryMethodId} Is Not Found");

            var SubTotal = OrderItems.Sum(I => I.Price * I.Quantity);

            var Order = new Order()
            {
                Address = OrderAddress,
                DeliveryMethod = DeliveryMethod,
                Items = OrderItems,
                SubTotal = SubTotal,
                UserEmail = Email
            };

            await _unitOfWork.GetRepository<Order, Guid>().AddAsync(Order);
            int Result = await _unitOfWork.SaveChangesAsync();

            if (Result == 0) return Error.Failure("Order.Failure", "Order Can Not Be Created");

            return _mapper.Map<OrderToReturnDTO>(Order);
        }

        private static OrderItem CreateOrderItem(Domain.Entities.BasketModule.BasketItem item, Product product)
        {
            return new OrderItem()
            {
                Product = new ProductItemOrdered()
                {
                    ProductId = product.Id,
                    PictureUrl = product.PictureUrl,
                    ProductName = product.Name
                },
                Price = product.Price,
                Quantity = item.Quantity
            };
        }
    }
}
