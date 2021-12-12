using Microsoft.Net.Http.Headers;
using PaymentGateway.HttpAggregator.Payloads.PaymentService.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetPaymentByIdAsync(string jwt, string paymentId)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            var result = await _httpClient.GetAsync($"payment/{paymentId}");
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> GetPaymentsAsync(string jwt)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            var result = await _httpClient.GetAsync("payment");
            return await result.Content.ReadAsStringAsync();
        }

        public async Task<string> ProcessPaymentAsync(string jwt, PaymentAttemptRequest paymentRequest)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);
            var result = await _httpClient.PostAsync("payment", JsonContent.Create(paymentRequest));
            return await result.Content.ReadAsStringAsync();
        }
    }
}
