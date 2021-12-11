using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bank.API.Application.Payloads.Requests
{
    public class PaymentApprovalRequest
    {
        public string CardNumber { get; set; }

        public int ExpiryMonth { get; set; }

        public int ExpiryYear { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public int Cvv { get; set; }
    }
}
