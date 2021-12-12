using PaymentGateway.HttpAggregator.Payloads.BankService.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Services
{
    public interface IBankService
    {
        Task<string> EvaluateTransaction(PaymentApprovalRequest request);
    }
}
