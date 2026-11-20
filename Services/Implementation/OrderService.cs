
using ECommerceManagementAPI.Data;
using ECommerceManagementAPI.DTOs;
using ECommerceManagementAPI.Helper;
using ECommerceManagementAPI.Models;
using ECommerceManagementAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceManagementAPI.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Create a new order
        public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
        {
            // Check if customer exists
            var customer = await _context.Customers
                .FindAsync(dto.CustomerId);

            if (customer == null)
            {
                throw new NotFoundException(
                    "Customer was not found");
            }

            // Get requested product IDs
            var productIds = dto.Items
                .Select(i => i.ProductId)
                .Distinct()
                .ToList();

            // Load products from database
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToListAsync();

            // Check if all products exist
            if (products.Count != productIds.Count)
            {
                throw new NotFoundException(
                    "One or more products were not found");
            }

            // Check if products are active
            if (products.Any(p => !p.IsActive))
            {
                throw new ConflictException(
                    "One or more products are inactive");
            }

            // Check stock for each product
            foreach (var product in products)
            {
                int requestedQuantity = dto.Items
                    .Where(i => i.ProductId == product.Id)
                    .Sum(i => i.Quantity);

                if (product.Stock < requestedQuantity)
                {
                    throw new ConflictException(
                        $"Insufficient stock for product: {product.Name}");
                }
            }

            // Create order
            var order = new Order
            {
                CustomerId = dto.CustomerId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            // Create order items using database prices
            foreach (var item in dto.Items)
            {
                var product = products.First(
                    p => p.Id == item.ProductId);

                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price
                });
            }

            // Update stock after all validations pass
            foreach (var product in products)
            {
                int requestedQuantity = dto.Items
                    .Where(i => i.ProductId == product.Id)
                    .Sum(i => i.Quantity);

                product.Stock -= requestedQuantity;
            }

            // Save order and stock changes together
            await using var transaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }

            return await GetByIdAsync(order.Id);
        }

        // Get order details by ID
        public async Task<OrderDto> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                throw new NotFoundException(
                    $"Order with ID {id} was not found");
            }

            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer!.FirstName
                    + " " + order.Customer.LastName,

                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product!.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    Total = i.Quantity * i.UnitPrice
                }).ToList(),

                TotalAmount = order.OrderItems.Sum(
                    i => i.Quantity * i.UnitPrice)
            };
        }
    }
}
