using System.ComponentModel.DataAnnotations;

namespace Payment.API.Application.Payloads.Requests
{
    
    public class PaymentAttemptRequest
    {
        [Required]
        public string CardNumber { get; set; }

        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public int ExpiryYear { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public int Cvv { get; set; }
    }
}