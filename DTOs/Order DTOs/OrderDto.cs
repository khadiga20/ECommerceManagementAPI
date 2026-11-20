
namespace ECommerceManagementAPI.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;

        public List<OrderItemDto> Items { get; set; } = new();

        public decimal TotalAmount { get; set; }
    }
}
