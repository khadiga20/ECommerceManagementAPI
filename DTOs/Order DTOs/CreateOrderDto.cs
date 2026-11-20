
using System.ComponentModel.DataAnnotations;

namespace ECommerceManagementAPI.DTOs
{
    public class CreateOrderDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "A valid customer is required")]
        public int CustomerId { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must contain at least one item")]
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
