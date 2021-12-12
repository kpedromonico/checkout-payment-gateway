using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Payloads.PaymentService.Responses
{
    public class PaymentConfirmationResponse
    {
        public string TransactionId { get; set; }

        public string CardNumber { get; set; }

        public bool? Approved { get; set; }

        public string Currency { get; set; }

        public decimal Amount { get; set; }        

        public DateTime TransactionDate { get; set; }
    }
}
