using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.HttpAggregator.Payloads.IdentityService.Requests
{
    public class AccountRegisterRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [MinLength(6)]
        [MaxLength(20)]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
    }
}