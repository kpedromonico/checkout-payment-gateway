using Microsoft.Extensions.Configuration;
using Payment.Domain.AggregatesModel.TransactionAggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Payment.API.Infrastructure.Services
{
    public class BankService : IBankService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BankService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool?> ProcessTransaction(Transaction transaction)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(transaction.Card),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient                
                .PostAsync(_configuration["BankService"], httpContent);

            return response.IsSuccessStatusCode;
        }
    }
}
