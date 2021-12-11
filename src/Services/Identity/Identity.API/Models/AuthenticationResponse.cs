using System.Collections.Generic;

namespace Identity.API.Models
{
    public class AuthenticationResponse
    {
        public string Token { get; set; }        

        public bool Success { get; set; }

        public IEnumerable<string> ErrorMessages { get; set; }
    }
}
