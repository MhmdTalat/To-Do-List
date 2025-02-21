namespace ECommerceProject.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }

        //public string PaymentStatus { get; set; } // E.g., "Pending", "Paid"
        //public string PaymentMethod { get; set; } // E.g., "Credit Card", "PayPal"
    }
}
