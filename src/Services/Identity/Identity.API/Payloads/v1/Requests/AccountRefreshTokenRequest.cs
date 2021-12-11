namespace Identity.API.Payloads.v1.Requests
{
    public class AccountRefreshTokenRequest
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}