using System.Collections.Generic;

namespace PaymentGateway.HttpAggregator.Payloads.IdentityService.Responses
{
    public class AccountResponse
    {
        public string Token { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}