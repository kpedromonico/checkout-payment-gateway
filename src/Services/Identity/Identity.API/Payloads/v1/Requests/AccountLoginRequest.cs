namespace Identity.API.Payloads.v1.Requests
{
    public class AccountLoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}