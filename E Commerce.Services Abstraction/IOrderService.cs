using E_Commerce.Shared.CommonResult;
using E_Commerce.Shared.DTOs.OrderDTOs;

namespace E_Commerce.Services_Abstraction
{
    public interface IOrderService
    {
        Task<Result<OrderToReturnDTO>> CreateOrderAsync(OrderDTO orderDTO, string Email);
    }
}
