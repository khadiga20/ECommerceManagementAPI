
using System.ComponentModel.DataAnnotations;

namespace ECommerceManagementAPI.DTOs
{
    public class CreateOrderItemDto
    {
        [Range(1, int.MaxValue,
            ErrorMessage = "A valid product ID is required")]
        public int ProductId { get; set; }

        [Range(1, 100000,
            ErrorMessage = "Quantity must be greater than zero")]
        public int Quantity { get; set; }
    }
}
