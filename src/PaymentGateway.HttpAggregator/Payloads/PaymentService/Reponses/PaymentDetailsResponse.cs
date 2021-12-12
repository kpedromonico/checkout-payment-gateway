using System;

namespace PaymentGateway.HttpAggregator.Payloads.PaymentService.Responses
{
    public class PaymentDetailsResponse
    {
        public string CardNumber { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public bool? Approved { get; set; }

        public DateTime TransactionDate { get; set; }
    }
}
