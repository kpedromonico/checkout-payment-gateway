using PaymentGateway.HttpAggregator.Payloads.IdentityService.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Services
{
    public interface IIdentityService
    {
        Task<string> Login(AccountLoginRequest request);

        Task<string> Register(AccountRegisterRequest request);
    }
}
