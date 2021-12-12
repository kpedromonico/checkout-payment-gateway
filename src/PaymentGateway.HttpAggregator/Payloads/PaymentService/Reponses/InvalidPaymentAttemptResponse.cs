using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentGateway.HttpAggregator.Payloads.PaymentService.Responses
{
    public class InvalidPaymentAttemptResponse
    {
        public string[] Errors { get; set; }
    }
}
