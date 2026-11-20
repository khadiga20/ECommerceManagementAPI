
using System.ComponentModel.DataAnnotations;

namespace ECommerceManagementAPI.DTOs
{
    public class CreateProductDto : IValidatableObject
    {
        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0.01, 1000000,
            ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Range(0, 100000,
            ErrorMessage = "Stock cannot be negative")]
        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;

        [Range(1, int.MaxValue,
            ErrorMessage = "A valid category is required")]
        public int CategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext)
        {
            if (Price > 100000 && Stock > 100)
            {
                yield return new ValidationResult(
                    "High-value products cannot have stock greater than 100",
                    new[] { nameof(Price), nameof(Stock) });
            }
        }
    }
}
