
using ECommerceManagementAPI.DTOs;

namespace ECommerceManagementAPI.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();

        Task<CategoryDto> CreateAsync(CreateCategoryDto dto);
    }
}
