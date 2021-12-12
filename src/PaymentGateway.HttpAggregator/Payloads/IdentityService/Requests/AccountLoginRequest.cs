namespace PaymentGateway.HttpAggregator.Payloads.IdentityService.Requests
{
    public class AccountLoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}