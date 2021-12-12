using PaymentGateway.HttpAggregator.Extensions;
using PaymentGateway.HttpAggregator.Payloads.IdentityService.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly HttpClient _httpClient;

        public IdentityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> Login(AccountLoginRequest request)
        {
            var result =  await _httpClient.PostAsync("auth/login", JsonContent.Create(request));
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> Register(AccountRegisterRequest request)
        {
            var result = await _httpClient.PostAsync("auth/register", JsonContent.Create(request));
            return await result.Content.ReadAsStringAsync();
        }
    }
}
