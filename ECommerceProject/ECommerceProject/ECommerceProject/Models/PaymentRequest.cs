namespace ECommerceProject.Models
{
    public class PaymentRequest
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string Cvv { get; set; }
        public decimal Amount { get; set; } // You can also include the amount for payment
    }
}
