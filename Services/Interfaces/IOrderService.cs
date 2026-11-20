
using ECommerceManagementAPI.DTOs;

namespace ECommerceManagementAPI.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto> CreateAsync(CreateOrderDto dto);

        Task<OrderDto> GetByIdAsync(int id);
    }
}
