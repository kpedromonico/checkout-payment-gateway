using System.Collections.Generic;

namespace Identity.API.Payloads.v1.Responses
{
    public class AccountResponse
    {
        public string Token { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}