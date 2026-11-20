
using ECommerceManagementAPI.DTOs;

namespace ECommerceManagementAPI.Services.Interfaces
{
    public interface IProductService
    {
        Task<PagedResultDto<ProductDto>> GetAllAsync(ProductQueryDto query);

        Task<ProductDto> GetByIdAsync(int id);

        Task<ProductDto> CreateAsync(CreateProductDto dto);

        Task<ProductDto> UpdateAsync(int id, UpdateProductDto dto);

        Task DeleteAsync(int id);
    }
}
