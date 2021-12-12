using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Payment.API.Application.Payloads.Reponses
{
    public class PaymentDetailsResponse
    {
        public string CardNumber { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public bool? Approved { get; protected set; }

        public DateTime TransactionDate { get; set; }
    }
}
