
using ECommerceManagementAPI.Data;
using ECommerceManagementAPI.DTOs;

using ECommerceManagementAPI.Helper;
using ECommerceManagementAPI.Models;
using ECommerceManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceManagementAPI.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all categories
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            return await _context.Categories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description
                })
                .ToListAsync();
        }

        // Create category
        public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
        {
            bool exists = await _context.Categories
                .AnyAsync(c => c.Name == dto.Name);

            if (exists)
            {
                throw new ConflictException(
                    "A category with this name already exists");
            }

            var category = new Category
            {
                Name = dto.Name,
                Description = dto.Description
            };

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}
