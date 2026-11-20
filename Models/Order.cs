
namespace ECommerceManagementAPI.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime OrderDate { get; set; }

        public string Status { get; set; } = "Pending";

       
        public int CustomerId { get; set; }

   
        public Customer? Customer { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();
    }
}
