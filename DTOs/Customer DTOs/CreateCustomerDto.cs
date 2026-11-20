
using System.ComponentModel.DataAnnotations;

namespace ECommerceManagementAPI.DTOs
{
    public class CreateCustomerDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }
    }
}
