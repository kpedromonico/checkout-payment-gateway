using PaymentGateway.HttpAggregator.Extensions;
using PaymentGateway.HttpAggregator.Payloads.BankService.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Services
{
    public class BankService : IBankService
    {
        private readonly HttpClient _httpClient;

        public BankService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> EvaluateTransaction(PaymentApprovalRequest request)
        {
            try
            {
                var httpContent = request.ConvertToStringContent();

                var response = await _httpClient
                    .PostAsync("transactions", httpContent);

                return await response.Content.ReadAsStringAsync();
            }
            catch
            {
                var result = "The Bank Service is unresponsive";
                return result;
            }
        }
    }
}
