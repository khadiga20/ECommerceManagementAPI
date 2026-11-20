
using ECommerceManagementAPI.Data;
using ECommerceManagementAPI.DTOs;

using ECommerceManagementAPI.Helper;
using ECommerceManagementAPI.Models;
using ECommerceManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceManagementAPI.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all products with search, filters, sorting and pagination
        public async Task<PagedResultDto<ProductDto>> GetAllAsync(
            ProductQueryDto query)
        {
            var products = _context.Products.AsQueryable();

            // Search by product name
            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                products = products.Where(p =>
                    p.Name.Contains(query.Search));
            }

            // Filter by category
            if (query.CategoryId.HasValue)
            {
                products = products.Where(p =>
                    p.CategoryId == query.CategoryId.Value);
            }

            // Filter by minimum price
            if (query.MinPrice.HasValue)
            {
                products = products.Where(p =>
                    p.Price >= query.MinPrice.Value);
            }

            // Filter by maximum price
            if (query.MaxPrice.HasValue)
            {
                products = products.Where(p =>
                    p.Price <= query.MaxPrice.Value);
            }

            // Filter by active status
            if (query.IsActive.HasValue)
            {
                products = products.Where(p =>
                    p.IsActive == query.IsActive.Value);
            }

            // Sorting using allowed fields only
            var sortOrder = query.SortOrder?.ToLower();

            products = query.SortBy?.ToLower() switch
            {
                "price" => sortOrder == "desc"
                    ? products.OrderByDescending(p => p.Price)
                    : products.OrderBy(p => p.Price),

                "name" => sortOrder == "desc"
                    ? products.OrderByDescending(p => p.Name)
                    : products.OrderBy(p => p.Name),

                _ => products.OrderBy(p => p.Name)
            };

            // Pagination
            int page = Math.Max(query.Page, 1);
            int pageSize = Math.Clamp(query.PageSize, 1, 100);

            int totalItems = await products.CountAsync();

            var data = await products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category!.Name
                })
                .ToListAsync();

            return new PagedResultDto<ProductDto>
            {
                Data = data,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = (int)Math.Ceiling(
                    (double)totalItems / pageSize)
            };
        }

        // Get product by ID
        public async Task<ProductDto> GetByIdAsync(int id)
        {
            var product = await _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    Stock = p.Stock,
                    IsActive = p.IsActive,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category!.Name
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                throw new NotFoundException(
                    $"Product with ID {id} was not found");
            }

            return product;
        }

        // Create product
        public async Task<ProductDto> CreateAsync(CreateProductDto dto)
        {
            var category = await _context.Categories
                .FindAsync(dto.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(
                    "Category was not found");
            }

            bool exists = await _context.Products
                .AnyAsync(p => p.Name == dto.Name);

            if (exists)
            {
                throw new ConflictException(
                    "A product with this name already exists");
            }

            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                IsActive = dto.IsActive,
                CategoryId = dto.CategoryId
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = category.Name
            };
        }

        // Update product
        public async Task<ProductDto> UpdateAsync(
            int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new NotFoundException(
                    $"Product with ID {id} was not found");
            }

            var category = await _context.Categories
                .FindAsync(dto.CategoryId);

            if (category == null)
            {
                throw new NotFoundException(
                    "Category was not found");
            }

            bool exists = await _context.Products
                .AnyAsync(p => p.Name == dto.Name && p.Id != id);

            if (exists)
            {
                throw new ConflictException(
                    "A product with this name already exists");
            }

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.Stock = dto.Stock;
            product.IsActive = dto.IsActive;
            product.CategoryId = dto.CategoryId;

            await _context.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CategoryId = product.CategoryId,
                CategoryName = category.Name
            };
        }

        // Delete product
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                throw new NotFoundException(
                    $"Product with ID {id} was not found");
            }

            _context.Products.Remove(product);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ConflictException(
                    "This product cannot be deleted because it is used in orders");
            }
        }
    }
}
